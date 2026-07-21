using RFBase.Exceptions;

namespace RFServices.Exceptions;

public class NoCurrentUserException()
    : HttpException(500, "No current user")
{ }
