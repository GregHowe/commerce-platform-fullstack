
namespace Core.CoreLib.Models.Exceptions
{
    public class DataNotFoundException : Exception
    {
        public DataNotFoundException(string? message)  : base(message) { }

        public DataNotFoundException(string? message, Exception? exception) : base(message, exception) { }
    }
}