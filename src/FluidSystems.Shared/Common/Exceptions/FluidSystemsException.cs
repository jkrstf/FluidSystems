namespace FluidSystems.Shared.Common.Exceptions
{
    public class FluidSystemsException : Exception
    {
        public FluidSystemsException() : base() {}
        public FluidSystemsException(string? message) : base(message) { }
        public FluidSystemsException(string? message, Exception? innerException) : base(message, innerException) { }
    }
}