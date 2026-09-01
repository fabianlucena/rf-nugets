using RFRBAC.Entities;

namespace RFRBAC.DTO;

public class RoleDTO(Role role) : RoleMinDTO(role)
{
    public string? Description { get; } = role.Description;
}
