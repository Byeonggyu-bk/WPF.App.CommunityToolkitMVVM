using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Core.Navigation;
using Wpf.Core.Navigation.Datas;
using Wpf.Datas;
using Wpf.Home;
using Wpf.Main;
using Wpf.Python;
using Wpf.Remark;

namespace Wpf.App
{
    /// <summary>
    /// View , ViewModel Mapping등록 클래스입니다.
    /// </summary>
    public class BootStarter : Navigator
    {
        protected override void RegisterViewModels(IViewModelMapper viewModelMapper)
        {
            viewModelMapper.Register<MainWindow, MainWindowViewModel>();
            viewModelMapper.Register<HomeView, HomeViewModel>();
            viewModelMapper.Register<DatasView, DatasViewModel>();
            viewModelMapper.Register<PythonView, PythonViewModel>();
            viewModelMapper.Register<RemarkView, RemarkViewModel>();
            viewModelMapper.Register<MainView, MainViewModel>();
        }
    }
}
