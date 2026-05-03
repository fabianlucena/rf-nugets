using RFHttpExceptions.Exceptions;

namespace RFBaseEntities.Exceptions
{
    public class CloneMethodMustBeOverridedInDerivatedClassException()
        : HttpException(500, "Clone method must be overrided in derivated class.")
    { }
}
