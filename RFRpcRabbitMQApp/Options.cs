using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RFRpcRabbitMQApp
{
    public class Options
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public SslOption Ssl { get; set; } = new();

        public Options(IConfiguration configuration)
        {

        }
    }
}
