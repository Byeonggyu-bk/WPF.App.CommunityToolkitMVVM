using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using System.IO;
using System.Windows.Media;
using Wpf.Assistance.Local.Message;

namespace Wpf.Datas
{
    public partial class DatasViewModel : ObservableRecipient
    {
        [ObservableProperty]
        string testText;
        [ObservableProperty]
        string excelPath;

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

        [RelayCommand]
        void SetPath()
        {
            var dlg = new OpenFolderDialog();

            string path = null;
            if (dlg.ShowDialog() is true)
            {
                path = dlg.FolderName;
                ExcelPath = path;
            }
        }

        [RelayCommand]
        void MakeExcelFile()
        {
            var path = ExcelPath + "Test.xlsx";

            if(File.Exists(path) is false)
            {
                AExcel.AExcel.Create(path);
            }

            using (var workBook = AExcel.AExcel.Open(path))
            {
                var worksheet = workBook["Sheet1"];
                worksheet[1, 1].Text = "TEST";

                workBook.Save();
            }

            //using (var workBook = AExcel.AExcel.Create(path))
            //{
            //    var worksheet = workBook["Sheet1"];
            //    worksheet[1, 1].Text = "TEST";

            //    workBook.Save();
            //}
        }
    }
}
