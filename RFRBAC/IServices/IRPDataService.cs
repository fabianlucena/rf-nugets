using RFAuth.Entities;
using RFRBAC.DTO;

namespace RFRBAC.IServices;

public interface IRPDataService
{
    Task<RPData> GetSingleBySession(Session session);
    Task<Session> DecorateSession(Session session);
}
