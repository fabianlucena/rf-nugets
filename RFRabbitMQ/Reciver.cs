using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RFRabbitMQ
{
    public class Reciver
        : IAsyncDisposable
    {
        private RabbitMQOptions Options { get; }
        private ConnectionFactory ConnectionFactory { get; }
        public IConnection? Connection { get; private set; }
        public IChannel? Channel { get; private set; }
        public string QueueName { get; private set; }
        public Action<BasicDeliverEventArgs> Handler { get; set;  }

        public Reciver(RabbitMQOptions options, string queueName, Action<BasicDeliverEventArgs> handler)
        {
            Options = options;
            QueueName = queueName;
            Handler = handler;
            ConnectionFactory = new ConnectionFactory
            {
                HostName = Options.HostName,
                Port = Options.Port,
                Ssl = Options.Ssl,
                UserName = Options.UserName,
                Password = Options.Password,
            };
        }

        public static Reciver Create(RabbitMQOptions options, string queueName, Action<BasicDeliverEventArgs> handler)
            => new(options, queueName, handler);

        public async Task StartAsync()
        {
            Connection = await ConnectionFactory.CreateConnectionAsync();
            Channel = await Connection.CreateChannelAsync();

            await Channel.QueueDeclareAsync(
                queue: Options.VirtualHost,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            var consumer = new AsyncEventingBasicConsumer(Channel);
            consumer.ReceivedAsync += (model, ea) =>
            {
                Handler?.Invoke(ea);

                return Task.CompletedTask;
            };

            await Channel.BasicConsumeAsync(
                queue: QueueName,
                autoAck: true,
                consumer: consumer
            );
        }

        public async ValueTask DisposeAsync()
        {
            if (Channel is not null)
                await Channel.CloseAsync();

            if (Connection is not null)
                await Connection.CloseAsync();

            GC.SuppressFinalize(this);
        }
    }
}
