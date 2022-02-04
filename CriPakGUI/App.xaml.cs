﻿using Autofac;
using CriPak.App_Start;
using System.Windows;

namespace CriPak
{
    /// <summary>
    /// App.xaml 的交互逻辑
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
            if(Container != null)
            {
                using(var scope = Container.BeginLifetimeScope())
                {
                    Run();
                }
            }
        }
    }
}
