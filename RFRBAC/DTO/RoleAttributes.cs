using RFRBAC.Entities;
using RFService.ILibs;

namespace RFRBAC.DTO
{
    public class RoleAttributes
        : Role
    {
        public IDataDictionary? Attributes { get; set; }
    }
}