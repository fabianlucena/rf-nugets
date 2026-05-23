using RFAuth.Entities;
using RFBase.ILibs;

namespace RFAuth.DTO
{
    public class SessionMinDTO(Session session)
    {
        public IDataDictionary? Data { get; set; } = session.DataResponse;
    }
}
