using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RFRpcRabbitApp.Attributes;
using RFRpcRabbitApp.Types;
using System.Reflection;

namespace RFRpcRabbitApp
{
    public class App
    {
        private Options Options { get; }
        private ConnectionFactory ConnectionFactory { get; }

        private IConnection? _connection = null;
        private IChannel? _channel = null;


        public App(Options? options = null)
        {
            Options = options ?? new Options();
            ConnectionFactory = new ConnectionFactory
            {
                HostName = Options.HostName,
                Port = Options.Port,
                Ssl = Options.Ssl,
                UserName = Options.UserName,
                Password = Options.Password,
            };
        }

        public static App Create(Options? options = null)
            => new(options);

        public static IEnumerable<Type> GetControllers()
        {
            var rpcControllerType = typeof(RpcController);
            return AppDomain.CurrentDomain
                .GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute(rpcControllerType) != null);
        }

        public static IEnumerable<MethodInfo> GetMethods(Type controller)
        {
            var queueType = typeof(Queue);
            return controller.GetMethods()
                .Where(m => m.GetCustomAttribute(queueType) != null);
        }

        public async Task MapControllersAsync()
        {
            _connection ??= await ConnectionFactory.CreateConnectionAsync();
            _channel ??= await _connection.CreateChannelAsync();

            var controllers = GetControllers();
            foreach (var controller in controllers)
            {
                var methods = GetMethods(controller);
                foreach (var method in methods)
                {
                    var queueAttribute = method.GetCustomAttribute<Queue>();
                    if (queueAttribute == null)
                        continue;

                    var queue = queueAttribute.Name?.Trim();
                    if (string.IsNullOrEmpty(queue))
                        continue;

                    var instance = Activator.CreateInstance(controller);
                    var methodInfo = controller.GetMethod(method.Name, BindingFlags.Public | BindingFlags.Instance);
                    if (methodInfo == null)
                        continue;

                    await _channel.QueueDeclareAsync(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);
                    await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new AsyncEventingBasicConsumer(_channel);
                    consumer.ReceivedAsync += async (sender, ea) =>
                    {
                        byte[] body = ea.Body.ToArray();
                        Response? response = null;
                        try
                        {
                            var instance = Activator.CreateInstance(controller);
                            var methodInfo = controller.GetMethod(method.Name, BindingFlags.Public | BindingFlags.Instance)
                                ?? throw new InvalidOperationException($"Method {method.Name} not found in controller {controller.Name}");

                            var asyncResult = methodInfo.Invoke(instance, [new Request(body)]);
                            object? result;
                            if (asyncResult is Task task)
                            {
                                await task.ConfigureAwait(false);
                                result = task.GetType().GetProperty("Result")?.GetValue(task);
                            }
                            else
                                result = asyncResult;

                            if (result != null)
                            {
                                response = result as Response
                                    ?? new Response(result);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine($" [.] {e.Message}");
                        }

                        AsyncEventingBasicConsumer cons = (AsyncEventingBasicConsumer)sender;
                        IChannel ch = cons.Channel;
                        if (!ch.IsOpen)
                        {
                            Console.WriteLine($" [.] Channel {queue} is closed or disposed.");
                            return;
                        }

                        IReadOnlyBasicProperties props = ea.BasicProperties;
                        var replyProps = new BasicProperties
                        {
                            CorrelationId = props.CorrelationId
                        };

                        await ch.BasicPublishAsync(
                            exchange: string.Empty,
                            routingKey: props.ReplyTo!,
                            mandatory: true,
                            basicProperties: replyProps,
                            body: response?.GetBytes()
                        );
                        await ch.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                    };

                    await _channel.BasicConsumeAsync(queue, false, consumer);
                }
            }

        }

        public void Run()
        {
            RunAsync()
                .GetAwaiter()
                .GetResult();
        }

        public async Task RunAsync()
        {
            await MapControllersAsync();
            Console.WriteLine(" [x] Awaiting RPC requests...");
            await Task.Delay(Timeout.Infinite);
        }
    }
}
