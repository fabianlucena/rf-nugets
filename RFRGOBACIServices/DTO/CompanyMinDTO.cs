using RFRGOBACEntities.Entities;

namespace RFRGOBACIServices.DTO
{
    public class CompanyMinDTO(Company company)
    {
        public Guid Uuid { get; } = company.Uuid;
        public string Name { get; } = company.Name;
    }
}
