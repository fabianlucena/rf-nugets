using RFAuthEntities.Entities;
using RFBaseEntities.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGCBACEntities.Entities
{
    [Table("SessionCompanies", Schema = "auth")]
    public class SessionCompany : NoIdEntity
    {
        [Key]
        public long SessionId { get; set; }
        public Session? Session { get; set; }

        public long CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
