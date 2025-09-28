using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf.Core.Navigation.Datas;
using Wpf.Datas;
using Wpf.Main;
using Wpf.Remark;

namespace Wpf.App
{
    public partial class MainWindowViewModel : ObservableRecipient
    {
        [ObservableProperty]
        IView mainView;

        public MainWindowViewModel(MainView view) 
        {
            MainView = view;
        }
    }
}
