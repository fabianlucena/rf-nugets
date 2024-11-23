using RFAuth.Entities;

namespace RFAuth.DTO
{
    public class UserAttributes : User
    {
        public IDictionary<string, object>? Attributes { get; set; }
    }
}
