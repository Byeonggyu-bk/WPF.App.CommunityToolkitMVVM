using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wpf.Remark
{
    public partial class RemarkViewModel : ObservableRecipient
    {
        [ObservableProperty]
        string testText;
        public RemarkViewModel() 
        {
            TestText = "Remark";
        }
    }
}
