using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Interface
{
    public interface IWorksheet
    {
        string Name { get; }

        ICell this[int colIndex, int rowIndex] { get; }

        IWorkbook Workbook { get; }
    }
}
