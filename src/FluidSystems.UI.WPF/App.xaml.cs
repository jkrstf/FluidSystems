using FluidSystems.Control;
using FluidSystems.Core;
using FluidSystems.Diagramming;
using FluidSystems.Infrastructure;
using FluidSystems.UI.WPF.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Windows;

namespace FluidSystems.UI.WPF
{
    public partial class App : Application
    {
        public static IHost Host { get; private set; }

        public App()
        {
            Host = Microsoft.Extensions.Hosting.Host
                .CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddInfrastructureServices();
                    services.AddCoreServices();
                    services.AddDiagrammingServices();
                    services.AddControlServices();

                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<DiagramViewModel>();
                    services.AddTransient<DashBoardViewModel>();
                })
                .Build();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Host.Start();

            var mainWindow = Host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();

            base.OnStartup(e);
        }
    }
}