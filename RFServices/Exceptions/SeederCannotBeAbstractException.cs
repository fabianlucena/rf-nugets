using RFBase.Exceptions;

namespace RFServices.Exceptions
{
    public class SeederCannotBeAbstractException(string name)
        : HttpException(500, "{0} seeder cannot be abstract", name)
    { }
}
