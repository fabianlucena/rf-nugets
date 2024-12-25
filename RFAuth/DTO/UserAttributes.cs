using RFAuth.Entities;
using RFService.ILibs;

namespace RFAuth.DTO
{
    public class UserAttributes
        : User
    {
        public IDataDictionary? Attributes { get; set; }
    }
}
