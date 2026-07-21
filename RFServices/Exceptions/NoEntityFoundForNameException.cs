using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class NoEntityFoundForNameException(string name)
        : HttpException(500, "No entity found matching for name {0}.", name)
    { }
}
