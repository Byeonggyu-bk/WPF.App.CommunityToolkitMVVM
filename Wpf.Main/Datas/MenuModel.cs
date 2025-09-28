using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Main.Datas
{
    public partial class MenuModel : ObservableRecipient
    {
        public delegate void MenuChangedHandler(MenuTypes type);
        public event MenuChangedHandler? MenuChanged;

        [ObservableProperty]
        private MenuTypes menuType;

        public MenuModel() { }

        partial void OnMenuTypeChanged(MenuTypes value)
        {
            MenuChanged?.Invoke(MenuType);
        }
    }

    public enum MenuTypes
    {
        Home,
        Datas,
        Remark,
    }
}
