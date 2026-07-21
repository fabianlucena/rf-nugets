using System.Text.Json.Serialization;

namespace RFOauth2Client.Entities
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;
    } 
}
