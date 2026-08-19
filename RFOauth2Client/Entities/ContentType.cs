using System.Text.Json.Serialization;

namespace RFOauth2Client.Entities;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ContentType
{
    [JsonPropertyName("application/x-www-form-urlencoded")]
    FormUrlEncoded,

    [JsonPropertyName("application/json")]
    Json,
}

public static class ContentTypeExtensions
{
    public static string ToString(this ContentType contentType)
    {
        return contentType switch
        {
            ContentType.FormUrlEncoded => "application/x-www-form-urlencoded",
            ContentType.Json => "application/json",
            _ => throw new ArgumentOutOfRangeException(nameof(contentType), contentType, null)
        };
    }
}