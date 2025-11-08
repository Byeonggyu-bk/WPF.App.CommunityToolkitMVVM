using AExcel.Interface;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Local
{
    internal class AWorksheet : IWorksheet
    {
        public ICell this[int colIndex, int rowIndex] => GetCell(colIndex, rowIndex);

        public string Name { get; set; }

        public IWorkbook Workbook => _workbook;

        public SheetData SheetDatas => GetSheetData();

        public Worksheet WorkSheetDatas => GetWorkSheetDatas();

        AWorkbook _workbook;
        WorksheetPart _workSheetPart;

        public AWorksheet(WorksheetPart worksheetpart, AWorkbook workbook)
        {
            _workSheetPart = worksheetpart;
            _workbook = workbook;
        }


        public void Save()
        {
            _workSheetPart.Worksheet.Save();
        }

        private SheetData GetSheetData()
        {
            return _workSheetPart.Worksheet.GetFirstChild<SheetData>();
        }
        private Worksheet GetWorkSheetDatas()
        {
            return _workSheetPart.Worksheet;
        }

        /// <summary>
        /// Index로 Cell 가져오기
        /// </summary>
        /// <param name="colIndex"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private ICell GetCell(int colIndex, int rowIndex)
        {
            return new ACell(this, colIndex, rowIndex);
        }
    }
}
