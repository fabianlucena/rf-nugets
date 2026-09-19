using RFBase.Exceptions;

namespace RFDapper.Exceptions;

public class ConnectionFactoryIsNotInitializedException()
    : HttpException(500, "ConnectionFactory is not initialized.")
{
}
