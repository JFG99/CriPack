using Autofac;
using CriPakComplete.App_Start;
using System.Windows;

namespace CriPakComplete
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer Container { get; set; }
        public App()
        {
            Container = DependencyInjectionConfig.GetContainer();
        }

        private void OnStartup(object sender, StartupEventArgs s)
        {
            if (Container != null)
            {
                using (var scope = Container.BeginLifetimeScope())
                {
                    var main = scope.Resolve<MainWindow>();
                    main.Show();
                }
            }
        }
    }
}
