using System.Text;

namespace RFRpcRabbitApp.Types
{
    public class Request(byte[] data)
    {
        public byte[] Data { get; } = data;

        public string GetString()
            => Encoding.UTF8.GetString(Data);

        public override string ToString()
            => GetString();
    }
}
