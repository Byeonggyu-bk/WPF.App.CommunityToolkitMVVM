using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Assistance.Local.Message;

namespace Wpf.Datas
{
    public partial class DatasViewModel : ObservableRecipient
    {
        [ObservableProperty]
        string testText;

        public DatasViewModel() 
        {
            IsActive = true;
            TestText = "Datas";
        }

        [RelayCommand]
        void ChangeColor()
        {
            Messenger.Send(new ColorMessage(Brushes.OrangeRed));
        }
    }
}
