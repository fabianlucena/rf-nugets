using RFAuth.Entities;
using RFBase.ILibs;
using RFBase.Libs;

namespace RFAuth.DTO
{
    public class SessionMinDTO(Session session)
    {
        public IDataDictionary? Data { get; set; } = new DataDictionary(session.ResponseData);
    }
}
