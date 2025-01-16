using RFHttpExceptions.Exceptions;
using System.Text.Json;

namespace RFDapper.Exceptions
{
    public class CannotCheckObjectValitityBecauseTheIdIsNotALongValueException()
        : HttpException(500, $"Cannot check object valitity because the IS is not a long value.")
    { }
}
