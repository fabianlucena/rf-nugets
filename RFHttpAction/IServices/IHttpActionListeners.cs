namespace RFHttpAction.IServices
{
    public interface IHttpActionListeners
    {
        void AddListener(string name, HttpActionListener listener);

        IEnumerable<HttpActionListener>? GetListeners(string name);
    }
}
