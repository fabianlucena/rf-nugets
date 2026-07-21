using RFHttpAction.Entities;

namespace RFHttpAction.DTO
{
    public class HttpActionTypeMinDTO(HttpActionType type)
    {
        public Guid Uuid { get; set; } = type.Uuid;

        public string Name { get; set; } = type.Name;

        public string Title { get; set; } = type.Title;
    }
}
