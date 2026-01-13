using System.Windows;

namespace FluidSystems.UI.WPF.Services
{
    public class MessageBoxService : IDialogService
    {
        public void ShowError(string message, string title = "Error") => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        public void ShowInformation(string message, string title = "Information") => MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
    }
}