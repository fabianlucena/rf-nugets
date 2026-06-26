global using HttpActionListener = System.Func<RFHttpAction.Entities.HttpAction, System.Threading.Tasks.Task>;

using RFHttpAction.IServices;
using RFRegisterService.Attributes;

namespace RFHttpAction.Services;

[RegisterService]
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
