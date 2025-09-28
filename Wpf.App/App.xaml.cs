using CommunityToolkit.Mvvm.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using Wpf.Core.Navigation.Datas;
using Wpf.Main;

namespace Wpf.App
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static BootStarter BootStarter { get; private set; }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            BootStarter = new BootStarter();
            BootStarter.Run();

            var window = Ioc.Default.GetRequiredService<MainWindow>();
            window.Show();
        }
    }

}
