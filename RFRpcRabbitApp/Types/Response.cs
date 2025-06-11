using System.Text;

namespace RFRpcRabbitApp.Types
{
    public class Response
    {
        public byte[] Data { get; }

        public Response(byte[] data)
            => Data = data;

        public Response(string text)
            => Data = Encoding.UTF8.GetBytes(text);

        public string GetString()
            => Encoding.UTF8.GetString(Data);

        public override string ToString()
            => GetString();
    }
}
