namespace RFLogger.Types
{
    public delegate Task<object?> LoggerProvider(LLevel level, string module, LAction action, string message, object? data = null, IDictionary<string, object>? options = null);
}
