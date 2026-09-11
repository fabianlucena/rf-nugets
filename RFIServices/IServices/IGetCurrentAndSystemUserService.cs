using RFEntities.Entities;

namespace RFIServices.IServices;

public interface IGetCurrentAndSystemUserService
{
    Task<User> GetSystemUserAsync();
    Task<long> GetSystemUserIdAsync();
    Task<User> GetCurrentUserAsync();
    Task<long> GetCurrentUserIdAsync();
    Task<User> GetCurrentOrSystemUserAsync();
    Task<long> GetCurrentOrSystemUserIdAsync();
}
