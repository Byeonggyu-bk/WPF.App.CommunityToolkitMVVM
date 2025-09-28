using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Wpf.Assistance.Local.Message;

namespace Wpf.Home
{
    public partial class HomeViewModel : ObservableRecipient, IRecipient<ColorMessage>
    {
        [ObservableProperty]
        string testText;

        [ObservableProperty]
        Brush randomColor;

        public HomeViewModel()
        {
            IsActive = true;
            TestText = "Home";
            RandomColor = Brushes.Blue;
        }

        public void Receive(ColorMessage message)
        {
            RandomColor = message.Color;
        }
    }
}
