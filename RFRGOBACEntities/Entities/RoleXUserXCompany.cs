using RFBaseEntities.Entities;
using RFRBACEntities.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace RFRGOBACEntities.Entities
{
    [Table("RolesXUsersXCompanies", Schema = "auth")]
    public class RoleXUserXCompany : CommonJoin
    {
        public long RoleId { get; set; }
        public Role? Role { get; set; }

        public long UserId { get; set; }
        public User? User { get; set; }

        public long CompanyId { get; set; }
        public Company? Company { get; set; }
    }
}
