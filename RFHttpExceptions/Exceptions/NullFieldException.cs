using RFBase.Exceptions;

namespace RFHttpExceptions.Exceptions;

public class NullFieldException(string name)
    : HttpException(400, "Field {0} cannot be null.", name)
{
}
