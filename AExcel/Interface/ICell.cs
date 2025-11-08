using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AExcel.Interface
{
    public interface ICell
    {
        int ColIndex { get; }

        int RowIndex { get; }

        string CellName { get; }

        string Text { get; set; }

        IWorksheet WorkSheet { get; }
    }
}
