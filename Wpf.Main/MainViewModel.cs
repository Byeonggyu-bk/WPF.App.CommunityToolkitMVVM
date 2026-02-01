using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Core.Navigation.Datas;
using Wpf.Datas;
using Wpf.Home;
using Wpf.Python;
using Wpf.Main.Datas;
using Wpf.Remark;

namespace Wpf.Main
{
    public partial class MainViewModel : ObservableRecipient
    {
        [ObservableProperty]
        IView contentView;

        [ObservableProperty]
        MenuModel menuModel;


        public MainViewModel() 
        {
            MenuModel = new MenuModel();
            MenuModel.MenuChanged += MenuModel_MenuChanged;

            MenuModel_MenuChanged(MenuModel.MenuType);
        }

        private void MenuModel_MenuChanged(MenuTypes type)
        {
           switch(type)
            {
                case MenuTypes.Home: ContentView = Ioc.Default.GetRequiredService<HomeView>(); break;
                case MenuTypes.Datas: ContentView = Ioc.Default.GetRequiredService<DatasView>(); break;
                case MenuTypes.Python: ContentView = Ioc.Default.GetRequiredService<PythonView>(); break;
                case MenuTypes.Remark: ContentView = Ioc.Default.GetRequiredService<RemarkView>(); break;
            }
        }
    }
}
