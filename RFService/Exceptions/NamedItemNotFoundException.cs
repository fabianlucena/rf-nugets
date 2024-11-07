namespace RFService.Exceptions
{
    public class NamedItemNotFoundException(string name) : HttpException(404, $"Item {name} not found.")
    {
    }
}
