using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace RFRabbitMQ
{
    public class RabbitMQOptions
    {
        public string HostName { get; set; } = "localhost";
        public int Port { get; set; } = 5672;
        public string UserName { get; set; } = "guest";
        public string Password { get; set; } = "guest";
        public string VirtualHost { get; set; } = "/";
        public SslOption Ssl { get; set; } = new();

        public RabbitMQOptions(IConfiguration configuration)
        {
            Configure(configuration);
        }

        public void Configure(IConfiguration configuration)
        {
            HostName = configuration["HostName"] ?? configuration["Hostname"] ?? configuration["Host"] ?? HostName;

            var textPort = configuration["Port"];
            if (!string.IsNullOrWhiteSpace(textPort) && int.TryParse(textPort, out var port))
                Port = port;

            UserName = configuration["UserName"] ?? UserName;
            Password = configuration["Password"] ?? Password;
            VirtualHost = configuration["VirtualHost"] ?? VirtualHost;
        }
    }
}
