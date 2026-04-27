using RFAuthEntities.Entities;
using RFBaseEntities.Libs;

namespace RFAuthServices.DTO
{
    public class CachedSession(Session session)
    {
        public string Token { get; } = session.Token;
        public long SessionId { get; } = session.Id;
        public DateTime ExpireAt { get; set; } = session.ExpireAt;
        public DataDictionary Items { get; } = [];
    }
}
