using AExcel.Function;
using AExcel.Interface;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Spreadsheet;

namespace AExcel.Local
{
    internal class ACell : ICell
    {
        public int ColIndex => GetColIndex();

        public int RowIndex => GetRowIndex();

        public string CellName => cell.CellReference;

        public string Text 
        { 
            get => GetValue(); 
            set => SetValue(value); 
        }

        public IWorksheet WorkSheet => _worksheet;

        Cell cell;
        Row row;
        AWorksheet _worksheet;

        private readonly object balanceLock = new object();

        public ACell(AWorksheet worksheet, int colIndex, int rowIndex)
        {
            _worksheet = worksheet;

            var cellName = ExcelConverter.ConvertIndexToColName(colIndex) + rowIndex.ToString();

            row = GetRow(cellName);
            SetCell(cellName, row);
        }

        public ACell(AWorksheet worksheet, string cellName)
        {
            _worksheet = worksheet;

            row = GetRow(cellName);
            SetCell(cellName, row);
        }


        /// <summary>
        /// 기존 Row 가져오거나 SheetData에 추가
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private Row GetRow(string cellName)
        {
            var rowIndex = (uint)(ExcelConverter.GetRowIndex(cellName));

            var ws = _worksheet;
            var row = new Row();

            var sheetData = ws.SheetDatas;
            lock (balanceLock)
            {
                if (sheetData?.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).Count() != 0)
                {
                    row = sheetData!.Elements<Row>().Where(r => r.RowIndex is not null && r.RowIndex == rowIndex).First();
                }
                else // 기존에 없던 row면 sheetData에 새로 추가
                {
                    row = new Row() { RowIndex = rowIndex };
                    // 추가될 row의 index보다 큰 rowIndex가 있는지 확인
                    var refRow = sheetData?.Elements<Row>().Where(r => r.RowIndex is not null && rowIndex < r.RowIndex).FirstOrDefault();
                    if (refRow == null) // 없으면 마지막에 추가
                    {
                        sheetData.Append(row);
                    }
                    else
                    {
                        sheetData.InsertBefore(row, refRow); // 있으면 refRow의 앞에 넣어줘야함
                    }
                }
            }

            return row;
        }

        /// <summary>
        /// 기존 Cell 가져오거나 Row에 추가
        /// </summary>
        /// <param name="cellName"></param>
        /// <param name="row"></param>
        private void SetCell(string cellName, Row row)
        {
            Cell? refCell = null;

            var cellsInRow = row.Elements<Cell>();

            bool isNewCell = true;

            foreach (var item in cellsInRow)
            {
                var tmpCellReference = item.CellReference?.Value;

                if (tmpCellReference == cellName) // row에 이미 있는 셀이면
                {
                    cell = item;
                    isNewCell = false;
                    break;
                }
            }

            if (isNewCell) // 기존에 없던 셀이면 row에 추가
            {
                foreach (var item in cellsInRow)
                {
                    var tmpCellReference = item.CellReference?.Value;

                    if (cellName.Length > tmpCellReference.Length)
                    {
                        continue;
                    }

                    if (string.Compare(tmpCellReference, cellName, true) > 0) // 기존에 있던 cell이 새로 넣을 셀 보다 뒤에 있으면
                    {
                        refCell = item; // 셀 넣을 기준으로 함
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellName };
                // datatype 및 cellvalue 초기화 안시켜주면 값 할당 안됨
                newCell.DataType = new EnumValue<CellValues> { };
                newCell.CellValue = new CellValue();
                row.InsertBefore(newCell, refCell);
                cell = newCell;
            }
        }

        /// <summary>
        /// ConIndex - openXml의 cell에서 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetColIndex()
        {
            return ExcelConverter.GetColumnIndex(cell.CellReference);
        }

        /// <summary>
        /// RowIndex - openXml의 cell에서 가져오기
        /// </summary>
        /// <returns></returns>
        private int GetRowIndex()
        {
            return ExcelConverter.GetRowIndex(cell.CellReference);
        }


        /// <summary>
        /// Cell TextValue Get
        /// </summary>
        /// <returns></returns>
        public string GetValue()
        {
            var cellValue = string.Empty;

            var wb = WorkSheet.Workbook as AWorkbook;

            var sharedStringTable = wb.WorkbookPart.SharedStringTablePart.SharedStringTable;

            if (!cell.HasAttributes)
            {
                return cellValue;
            }

            if (cell.DataType != null)
            {
                switch (cell.DataType.InnerText)
                {
                    case "s": // SharedString
                        {
                            var index = int.Parse(cell.CellValue.Text);
                            var stringItem = sharedStringTable.Elements().ElementAt(index);
                            if ((stringItem as SharedStringItem).InnerText != null)
                            {
                                cellValue = (stringItem as SharedStringItem).InnerText;
                            }
                        }
                        break;
                    case "str": // String
                        {
                            cellValue = cell.CellValue.Text;
                        }
                        break;
                    case "inlineStr": // InnerString
                        {
                            cellValue = cell.InlineString.Text.InnerText;
                        }
                        break;
                    default:
                        break;
                }
            }
            else
            {
                if (cell.CellValue != null)
                {
                    cellValue = cell.CellValue.Text;
                }

            }

            return cellValue;
        }

        /// <summary>
        /// cell value set
        /// </summary>
        /// <param name="obj"></param>
        public void SetValue(string val)
        {
            var cellValue = val;

            // 셀 value 지울 경우
            if (string.IsNullOrEmpty(val))
            {
                cell.DataType = null;
                cell.CellValue = new CellValue();
                return;
            }

            // 타입 확인
            var numCheck = double.TryParse(cellValue, out double numValue);
            if (!numCheck)
            {
                SetStringValue(cellValue);
            }
            else
            {
                // 숫자는 데이터타입 없이 그냥 값만 넣으면 됨
                if (cell.DataType != null)
                {
                    // 기존 데이터 타입이 있는 경우 초기화 해줘야하지만
                    // 데이터 타입이 없는 상태에서 한번 더 초기화하면 값 제대로 입력되지 않음..
                    cell.DataType = new EnumValue<CellValues> { };
                }
                cell.CellValue = new CellValue(numValue);
            }
        }


        /// <summary>
        /// string type set
        /// </summary>
        /// <param name="inputValue"></param>
        private void SetStringValue(string inputValue)
        {
            // 기존 셀 데이터타입 없으면 SharedString으로 넣어주기 => 데이터 관리에 효율적
            if (cell.DataType.InnerText == null)
            {
                cell.DataType = new EnumValue<CellValues> { };
                cell.DataType.InnerText = "s";
            }

            if (cell.DataType.InnerText == "s") // SharedString
            {
                var stringValue = SetSharedStringValue(inputValue);
                cell.CellValue = new CellValue(stringValue);
            }
            else if (cell.DataType.InnerText == "str" || cell.DataType.InnerText == "inlineStr") // String, InnerString
            {
                cell.CellValue = new CellValue(inputValue);
            }
        }


        /// <summary>
        /// SharedStringType 추가
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string SetSharedStringValue(string text)
        {
            var wb = WorkSheet.Workbook as AWorkbook;
            var sharedStringTable = wb.WorkbookPart.SharedStringTablePart.SharedStringTable;
            var sharedStringItemOrigin = sharedStringTable.Descendants<SharedStringItem>();

            var indexOfSharedStringTable = string.Empty;
            int i = 0;
            foreach (SharedStringItem item in sharedStringItemOrigin) // SharedStringTable에서 검색해서 있으면 index 반환 없으면 추가하고 반환
            {
                if (item.InnerText == text)
                {
                    indexOfSharedStringTable = i.ToString();
                    return indexOfSharedStringTable;
                }

                i++;
            }

            indexOfSharedStringTable = i.ToString();
            sharedStringTable.AppendChild(new SharedStringItem(new Text(text)));
            sharedStringTable.Save();

            return indexOfSharedStringTable;
        }
    }
}
