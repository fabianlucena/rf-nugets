using RFAuthEntities.Entities;
using RFBaseEntities.ILibs;
using RFBaseIServices.DTO;

namespace RFAuthIServices.DTO
{
    public class SessionResponse(Session session)
    {
        public string AuthorizationToken { get; } = session.AuthorizationToken;
        public DateTime ExpireAt { get; } = session.ExpireAt;
        public string AutoLoginToken { get; } = session.AutoLoginToken;
        public string DeviceToken { get; } = session.Device?.Token ?? string.Empty;
        public UserMinDTO? User { get; set; } = session.User != null ? new UserMinDTO(session.User): null;
        public IDataDictionary? Data { get; set; } = session.DataResponse;
    }
}
