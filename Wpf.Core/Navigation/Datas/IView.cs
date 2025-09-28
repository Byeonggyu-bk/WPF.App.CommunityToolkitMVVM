using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Core.Navigation.Datas
{
    /// <summary>
    /// View(UserControl)의 인터페이스 입니다.
    /// </summary>
    public interface IView
    {
        object? DataContext { get; set; }
    }
}
