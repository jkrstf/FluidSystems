using FluidSystems.UI.WPF.ViewModels;
using System.Windows;

namespace FluidSystems.UI.WPF
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