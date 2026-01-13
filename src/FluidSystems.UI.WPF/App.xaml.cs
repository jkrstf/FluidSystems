using FluidSystems.Control;
using FluidSystems.Core;
using FluidSystems.Diagramming;
using FluidSystems.Infrastructure;
using FluidSystems.UI.WPF.Services;
using FluidSystems.UI.WPF.ViewModels.ControlPanels;
using FluidSystems.UI.WPF.ViewModels.Diagnostics;
using FluidSystems.UI.WPF.ViewModels.Diagrams;
using FluidSystems.UI.WPF.ViewModels.SystemLogs;
using FluidSystems.UI.WPF.ViewModels.Main;
using FluidSystems.UI.WPF.Views.Main;
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

                    services.AddSingleton<IDialogService, MessageBoxService>();
                    services.AddSingleton<MainWindow>();
                    services.AddSingleton<MainViewModel>();
                    services.AddTransient<HomeViewModel>();
                    services.AddTransient<DiagramViewModel>();
                    services.AddTransient<DiagnosticsViewModel>();
                    services.AddTransient<ControlPanelViewModel>();
                    services.AddTransient<FillChamberViewModel>();
                    services.AddTransient<EmptyingChamberViewModel>();
                    services.AddTransient<ManualControlViewModel>();
                    services.AddTransient<LogsViewModel>();
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