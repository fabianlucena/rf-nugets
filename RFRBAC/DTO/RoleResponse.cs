using RFService.ILibs;
using System.Text.Json.Serialization;

namespace RFRBAC.DTO
{
    public class RoleResponse
    {
        public Guid Uuid { get; set; }

        public bool IsEnabled { get; set; }

        public required string Name { get; set; }

        public required string Title { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public IDataDictionary? Attributes { get; set; }
    }
}