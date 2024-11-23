using RFRBAC.Entities;

namespace RFRBAC.DTO
{
    public class RoleAttributes
        : Role
    {
        public IDictionary<string, object>? Attributes { get; set; }
    }
}