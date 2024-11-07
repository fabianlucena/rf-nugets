namespace RFService.Exceptions
{
    public class NullFieldException(string name) : HttpException(400, $"Field {name} cannot be null.")
    {
    }
}
