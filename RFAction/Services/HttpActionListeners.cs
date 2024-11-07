using RFHttpAction.IServices;

namespace RFHttpAction.Services
{
    public class HttpActionListeners : IHttpActionListeners
    {
        static readonly Dictionary<string, List<HttpActionListener>> listeners = [];

        public void AddListener(string name, HttpActionListener decorator)
        {
            if (!listeners.TryGetValue(name, out var list))
                listeners[name] = list = [];

            list.Add(decorator);
        }

        public IEnumerable<HttpActionListener>? GetListeners(string name)
        {
            if (!listeners.TryGetValue(name, out var list))
                return null;

            return list;
        }
    }
}
