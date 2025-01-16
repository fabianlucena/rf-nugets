using RFHttpExceptions.Exceptions;
using System.Text.Json;

namespace RFDapper.Exceptions
{
    public class CannotCheckObjectValitityBecauseTheObjectIsNullException()
        : HttpException(500, $"Cannot check object valitity because the object is null.")
    { }
}
