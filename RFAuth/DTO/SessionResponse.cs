using RFAuth.Entities;
using RFIServices.DTO;

namespace RFAuth.DTO
{
    public class SessionResponse(Session session)
    {
        public string AuthorizationToken { get; } = session.AuthorizationToken;
        public DateTime ExpireAt { get; } = session.ExpireAt;
        public string AutoLoginToken { get; } = session.AutoLoginToken;
        public string DeviceToken { get; } = session.Device?.Token ?? string.Empty;
        public UserMinDTO? User { get; set; } = session.User != null ? new UserMinDTO(session.User): null;
        public SessionData Data { get; set; } = session.DataResponse;
    }
}
