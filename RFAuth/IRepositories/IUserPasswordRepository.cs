using RFAuth.Entities;
using RFIRepositories.IRepositories;

namespace RFAuth.IRepositories
{
    public interface IUserPasswordRepository : INoIdEntityRepository<UserPassword>
    {
    }
}