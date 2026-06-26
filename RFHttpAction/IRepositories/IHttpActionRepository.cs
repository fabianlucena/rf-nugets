using RFHttpAction.Entities;
using RFIRepositories.IRepositories;

namespace RFHttpAction.IRepositories
{
    public interface IHttpActionRepository : IAuditableEntityRepository<HttpAction>
    {
    }
}