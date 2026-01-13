using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluidSystems.Core.Models.Layout;
using FluidSystems.Core.Models.System;
using FluidSystems.Core.Services.Interfaces;
using FluidSystems.Shared.Common.Exceptions;
using System.Reflection;
using System.Windows;

namespace FluidSystems.UI.WPF.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        private FluidSystem _system;
        private FluidSystemLayout _layout;

        private IFluidSystemLoader _systemLoader;
        private IFluidSystemLayoutLoader _systemLayoutLoader;

        [ObservableProperty] private string _title;

        [ObservableProperty] private string _systemName;
        [ObservableProperty] private string _systemVersion;
        [ObservableProperty] private string _layoutVersion;
        [ObservableProperty] private string _systemDescription;
        [ObservableProperty] private DashBoardViewModel _dashBoardViewModel;


        public MainViewModel(DashBoardViewModel dashBoardViewModel, IFluidSystemLoader systemLoader, IFluidSystemLayoutLoader systemLayoutLoader)
        {
            _title = GetAppTitleWithVersion();

            _dashBoardViewModel = dashBoardViewModel;
            _systemLoader = systemLoader;
            _systemLayoutLoader = systemLayoutLoader;

            InitializeAsync();  
        }

        public async Task InitializeAsync()
        {
            var systemLoaderResult = await _systemLoader.LoadAsync(@"..\..\..\..\Resources\systems\manifold-01.json");

            if (systemLoaderResult.IsSuccess)
            {
                _system = systemLoaderResult.Value.Content;
                SystemName = systemLoaderResult.Value.Content.Name;
                SystemVersion = systemLoaderResult.Value.Content.Version;
                SystemDescription = systemLoaderResult.Value.Content.Description;
            }
            else if (systemLoaderResult.Exception is FluidSystemsException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Exit();
            }


            var systemLayoutLoaderResult = await _systemLayoutLoader.LoadAsync(@"..\..\..\..\Resources\layouts\manifold-01_layout.json");

            if (systemLayoutLoaderResult.IsSuccess)
            {
                _layout = systemLayoutLoaderResult.Value.Content;
                LayoutVersion = systemLayoutLoaderResult.Value.Metadata.Version;
            }
            else if (systemLayoutLoaderResult.Exception is FluidSystemsException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Exit();
            }

            if (_system != null && _layout != null)
            {
                _dashBoardViewModel.Update(_system, _layout);
            }
        }

        private string GetAppTitleWithVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            if (version == null) return "Manifold Control";
            return $"Manifold Control v{version.Major}.{version.Minor}";
        }

        [RelayCommand] private void Exit() => Application.Current.Shutdown();
        [RelayCommand] private void About() => MessageBox.Show(Title, "About", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}
