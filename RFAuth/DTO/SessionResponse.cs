using RFAuth.Entities;
using RFBase.Libs;
using RFIServices.DTO;

namespace RFAuth.DTO
{
    public class SessionResponse : DataDictionary
    {
        public SessionResponse(Session session)
        {
            this["authorizationToken"] = session.AuthorizationToken;
            this["expireAt"] = session.ExpireAt;
            this["autoLoginToken"] = session.AutoLoginToken;
            this["deviceToken"] = session.Device?.Token ?? string.Empty;
            this["user"] = session.User != null ? new UserMinDTO(session.User) : null;
            foreach (var kvp in session.ResponseData)
                this[kvp.Key] = kvp.Value;
        }
    }
}
