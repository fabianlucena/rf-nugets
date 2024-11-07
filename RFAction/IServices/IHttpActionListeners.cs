global using HttpActionListener = System.Func<RFHttpAction.Entities.HttpAction, System.Threading.Tasks.Task>;

namespace RFHttpAction.IServices
{
    public interface IHttpActionListeners
    {
        void AddListener(string name, HttpActionListener listener);

        IEnumerable<HttpActionListener>? GetListeners(string name);
    }
}
