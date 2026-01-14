namespace FluidSystems.UI.WPF.Services
{
    public interface IDialogService
    {
        void ShowError(string message, string title = "Error");
        void ShowInformation(string message, string title = "Information");
    }
}
