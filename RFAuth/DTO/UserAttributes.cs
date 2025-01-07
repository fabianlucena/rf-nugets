using RFAuth.Entities;
using RFService.Libs;

namespace RFAuth.DTO
{
    public class UserAttributes
        : User
    {
        public DataDictionary? Attributes { get; set; }
    }
}
