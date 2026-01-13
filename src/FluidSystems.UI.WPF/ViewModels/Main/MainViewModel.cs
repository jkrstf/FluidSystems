using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Control.Core;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.UI.WPF.Resources;
using FluidSystems.UI.WPF.Services;
using System.IO;
using System.Reflection;
using System.Windows;

namespace FluidSystems.UI.WPF.ViewModels.Main
{
    public partial class MainViewModel : ObservableObject
    {
        private readonly IDialogService _dialogService;
        private readonly IFluidSystemLoader _systemLoader;
        private readonly IFluidSystemLayoutLoader _systemLayoutLoader;
        private readonly SimulationContext _context;

        [ObservableProperty] private HomeViewModel _homeViewModel;
        [ObservableProperty] private string _title;

        [ObservableProperty] private string _systemName;
        [ObservableProperty] private string _systemVersion;
        [ObservableProperty] private string _layoutVersion;
        [ObservableProperty] private string _systemDescription;



        public MainViewModel(HomeViewModel homeViewModel, IDialogService dialogService, IFluidSystemLoader systemLoader, IFluidSystemLayoutLoader systemLayoutLoader, SimulationContext context)
        {
            _homeViewModel = homeViewModel;
            _dialogService = dialogService;
            _systemLoader = systemLoader;
            _systemLayoutLoader = systemLayoutLoader;
            _context = context;
            
            Title = GetAppTitleWithVersion();
        }

        [RelayCommand]
        public async Task InitializeAsync()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string systemPath = Path.Combine(baseDir, "Resources", "systems", "manifold-01.json");
            string layoutPath = Path.Combine(baseDir, "Resources", "layouts", "manifold-01_layout.json");

            await LoadSystemAsync(systemPath, layoutPath);
        }

        private async Task<bool> LoadSystemAsync(string systemPath, string layoutPath)
        {
            try
            {
                var systemTask = _systemLoader.LoadAsync(systemPath);
                var layoutTask = _systemLayoutLoader.LoadAsync(layoutPath);

                await Task.WhenAll(systemTask, layoutTask);

                var systemResult = await systemTask;
                var layoutResult = await layoutTask;

                if (!systemResult.IsSuccess)
                {
                    _dialogService.ShowError(systemResult.Exception?.Message);
                    return false;
                }

                if (!layoutResult.IsSuccess)
                {
                    _dialogService.ShowError(layoutResult.Exception?.Message);
                    return false;
                }

                var system = systemResult.Value.Content;
                var layout = layoutResult.Value.Content;

                SystemName = system.Name;
                SystemVersion = system.Version;
                SystemDescription = system.Description;
                LayoutVersion = layoutResult.Value.Metadata.Version;

                _context.Initialize(system, layout);
                return true;
            }
            catch (Exception ex)
            {
                _dialogService.ShowError(Texts.CouldNotInitializeSystem);
                return false;
            }
        }

        private string GetAppTitleWithVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null) return "Manifold Control";
            return $"Manifold Control v{version.Major}.{version.Minor}";
        }

        [RelayCommand] private void About() => _dialogService.ShowInformation(Title, "About");
        [RelayCommand] private void Exit() => Application.Current.Shutdown();
    }
}