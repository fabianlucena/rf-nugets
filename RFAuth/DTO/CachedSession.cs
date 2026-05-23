using RFAuth.Entities;
using RFBase.Libs;

namespace RFAuth.DTO
{
    public class CachedSession(Session session)
    {
        public string AuthorizationToken { get; } = session.AuthorizationToken;
        public long SessionId { get; } = session.Id;
        public DateTime ExpireAt { get; set; } = session.ExpireAt;
        public DataDictionary Items { get; } = [];
    }
}
