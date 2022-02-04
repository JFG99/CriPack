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

        public void Execute()
        {
            if (Container != null)
            {
                using (var scope = Container.BeginLifetimeScope())
                {
                    Run();
                }
            }
        }
    }
}
