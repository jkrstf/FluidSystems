namespace FluidSystems.UI.WPF.Models
{
    public class LogModel
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; }

        public LogModel(string message)
        {
            Timestamp = DateTime.Now;
            Message = message;
        }
    }
}