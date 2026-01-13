using FluidSystems.UI.WPF.ViewModels.Main;
using System.Windows;

namespace FluidSystems.UI.WPF.Views.Main
{
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel vm)
        {
            InitializeComponent();
            DataContext = vm;
        }
    }
}