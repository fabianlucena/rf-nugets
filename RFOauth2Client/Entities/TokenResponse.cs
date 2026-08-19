using System.Text.Json.Serialization;

namespace RFOauth2Client.Entities
{
    public class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; } = default;

        [JsonPropertyName("refresh_expires_in")]
        public long RefreshExpiresIn { get; set; } = default;

        [JsonPropertyName("id_token")]
        public string IdToken { get; set; } = string.Empty;
    } 
}
