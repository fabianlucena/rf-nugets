using RFHttpExceptions.Exceptions;
using System.Text.Json;

namespace RFDapper.Exceptions
{
    public class CannotCheckObjectValitityBecauseTheObjectDoesNotHaveIdException()
        : HttpException(500, $"Cannot check object valitity because the object does not have ID.")
    { }
}
