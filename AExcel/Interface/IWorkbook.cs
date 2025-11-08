using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Interface
{
    public interface IWorkbook : IDisposable
    {
        IEnumerable<IWorksheet> Worksheets { get; }

        IWorksheet this[string sheetname] { get; }

        bool Save();
    }
}
