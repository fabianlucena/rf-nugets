using RFAuth.Entities;

namespace RFAuth.DTO
{
    public class SessionMinDTO(Session session)
    {
        public SessionData? Data { get; set; } = session.DataResponse;
    }
}
