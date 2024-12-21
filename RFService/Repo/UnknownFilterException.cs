
namespace RFService.Repo
{
    [Serializable]
    internal class UnknownFilterException : Exception
    {
        public UnknownFilterException()
        {
        }

        public UnknownFilterException(string? message) : base(message)
        {
        }

        public UnknownFilterException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}