using System.Text.Json.Serialization;

namespace RFOauth2Client.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Method
{
    GET,
    POST,
    PUT,
    PATCH,
    DELETE,
}

public static class MethodExtensions
{
    public static string ToString(this Method method)
    {
        return method switch
        {
            Method.GET => "GET",
            Method.POST => "POST",
            Method.PUT => "PUT",
            Method.PATCH => "PATCH",
            Method.DELETE => "DELETE",
            _ => throw new ArgumentOutOfRangeException(nameof(method), method, null)
        };
    }
}