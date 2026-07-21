using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class SeederCannotBeAGenericTypeException(string name)
        : HttpException(500, "{0} seeder cannot be a generic type", name)
    { }
}
