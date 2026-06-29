using RFHttpAction.Entities;
using RFIServices.DTO;
using System.Text.Json;

namespace RFHttpAction.DTO
{
    public class HttpActionResponse(HttpAction action)
    {
        public Guid Uuid { get; set; } = action.Uuid;

        public HttpActionTypeMinDTO? Type { get; set; } = action.Type is null ? null : new HttpActionTypeMinDTO(action.Type);

        public string Token { get; set; } = action.Token;

        public DateTime CreatedAt { get; set; } = action.CreatedAt;

        public UserMinDTO? CreatedBy { get; set; } = action.CreatedBy is null ? null : new UserMinDTO(action.CreatedBy);

        public DateTime UpdatedAt { get; set; } = action.UpdatedAt;

        public UserMinDTO? UpdatedBy { get; set; } = action.UpdatedBy is null ? null : new UserMinDTO(action.UpdatedBy);

        public DateTime? ClosedAt { get; set; } = action.ClosedAt;

        public object? Data { get; set; } = JsonSerializer.Deserialize<object>(action.Data);
    }
}
