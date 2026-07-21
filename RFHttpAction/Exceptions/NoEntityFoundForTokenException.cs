using RFBase.Exceptions;

namespace RFHttpAction.Exceptions
{
    public class NoEntityFoundForTokenException(string token)
        : HttpException(500, "No entity found matching for token {0}.", token)
    { }
}
