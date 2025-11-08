using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AExcel.Function
{
    internal class ExcelConverter
    {

        internal static int GetColumnIndex(string cellRefrenece)
        {
            var cellName = GetIndex(cellRefrenece)[1];

            var index = ConvertColNameToIndex(cellName);

            return index;
        }

        internal static int GetRowIndex(string cellRefrenece)
        {
            var index = GetIndex(cellRefrenece)[2];

            return int.Parse(index);
        }

        internal static List<string> GetIndex(string cellRefrenece)
        {
            Regex regex = new Regex("([A-Za-z]+)");

            return regex.Split(cellRefrenece).ToList();
        }

        internal static string MakeCellReference(int ColIndex, int RowIndex)
        {
            var colName = ConvertIndexToColName(ColIndex);

            return colName + RowIndex.ToString();
        }

        internal static int ConvertColNameToIndex(string cellName)
        {
            var maxColumn = "XFD";

            var maxColumnIndex = CalcColIndex(maxColumn.ToCharArray()); // 16384

            var columntoChar = cellName.ToCharArray();

            var result = CalcColIndex(columntoChar);

            if (result <= 0 || maxColumnIndex < result)
            {
                return 0;
            }

            return result;
        }

        internal static string ConvertIndexToColName(int colIndex)
        {
            var colName = string.Empty;

            var colIndexManager = new ColumnIndexManager();
            var defaltIndexCount = colIndexManager.defaultIndexes.Count;

            var maxColumn = "XFD";
            var maxColumnIndex = CalcColIndex(maxColumn.ToCharArray()); // 16384
            if (maxColumnIndex < colIndex)
            {
                return null;
            }

            var twoCountMax = "ZZ";
            var twoCountMaxIndex = CalcColIndex(twoCountMax.ToCharArray());

            if (colIndex <= defaltIndexCount) // A~Z
            {
                return colIndexManager.IndexToColName(colIndex).ToString();
            }
            else if (defaltIndexCount < colIndex && colIndex <= twoCountMaxIndex) // AA ~ ZZ
            {
                for (int i = 1; i <= defaltIndexCount; i++)
                {
                    var firstIndex = i;

                    for (int j = 1; j <= defaltIndexCount; j++)
                    {
                        var lastIndex = j;

                        var firstColName = colIndexManager.IndexToColName(firstIndex);
                        var lastColName = colIndexManager.IndexToColName(lastIndex);

                        var colums = new char[2] { firstColName, lastColName };

                        if (CalcColIndex(colums) == colIndex)
                        {
                            colName = firstColName.ToString() + lastColName.ToString();
                            break;
                        }
                    }
                }
            }
            else
            {
                for (int i = 1; i <= defaltIndexCount; i++)
                {
                    var firstIndex = i;

                    for (int j = 1; j <= defaltIndexCount; j++)
                    {
                        var secondIndex = j;

                        for (int k = 1; k <= defaltIndexCount; k++)
                        {
                            var lastIndex = k;

                            var firstColName = colIndexManager.IndexToColName(firstIndex);
                            var secondColName = colIndexManager.IndexToColName(secondIndex);
                            var lastColName = colIndexManager.IndexToColName(lastIndex);

                            var colums = new char[3] { firstColName, lastColName, lastColName };

                            if (CalcColIndex(colums) == colIndex)
                            {
                                colName = firstColName.ToString() + secondColName.ToString() + lastColName.ToString();

                                break;
                            }
                        }
                    }
                }
            }

            return colName;
        }

        private static int CalcColIndex(char[] columns)
        {
            var result = 0;

            if (columns.Length > 3)
            {
                return result;
            }

            var colIndexManager = new ColumnIndexManager();
            var defaltIndexCount = colIndexManager.defaultIndexes.Count;

            if (columns.Length == 1) // A ~ Z
            {
                result = colIndexManager.ColNameToIndex(columns[0]);
            }
            else if (columns.Length == 2) // AA ~ ZZ
            {
                var firstIndex = colIndexManager.ColNameToIndex(columns[0]);
                var lastIndex = colIndexManager.ColNameToIndex(columns[1]);

                result = (defaltIndexCount * firstIndex) + lastIndex;
            }
            else // AAA ~ XFD
            {
                var firstIndex = colIndexManager.ColNameToIndex(columns[0]);
                var secondIndex = colIndexManager.ColNameToIndex(columns[1]);
                var lastIndex = colIndexManager.ColNameToIndex(columns[2]);

                result = ((defaltIndexCount * defaltIndexCount) * firstIndex) + (defaltIndexCount * secondIndex) + lastIndex;
            }

            return result;
        }
    }
}
