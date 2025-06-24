﻿using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RFRabbitMQ;
using RFRpcRabbitMQApp.Types;
using System.Collections.Concurrent;

namespace RFRpcRabbitMQApp
{
    public class RpcClient
        : IAsyncDisposable
    {
        private RabbitMQOptions Options { get; }
        private ConnectionFactory ConnectionFactory { get; }
        private ConcurrentDictionary<string, TaskCompletionSource<Response>> CallbackMapper { get; } = [];

        public RpcClient(RabbitMQOptions options)
        {
            Options = options;
            ConnectionFactory = new ConnectionFactory
            {
                HostName = Options.HostName,
                Port = Options.Port,
                Ssl = Options.Ssl,
                UserName = Options.UserName,
                Password = Options.Password,
            };
        }

        public static RpcClient Create(RabbitMQOptions options)
            => new(options);

        private IConnection? Connection;
        private IChannel? Channel;
        private string? ReplyQueueName;

        public async Task StartAsync()
        {
            Connection = await ConnectionFactory.CreateConnectionAsync();
            Channel = await Connection.CreateChannelAsync();

            QueueDeclareOk queueDeclareResult = await Channel.QueueDeclareAsync();
            ReplyQueueName = queueDeclareResult.QueueName;
            var consumer = new AsyncEventingBasicConsumer(Channel);

            consumer.ReceivedAsync += (model, ea) =>
            {
                string? correlationId = ea.BasicProperties.CorrelationId;
                if (!string.IsNullOrEmpty(correlationId)
                    && CallbackMapper.TryRemove(correlationId, out var tcs)
                )
                    tcs.TrySetResult(new Response(ea.Body.ToArray()));

                return Task.CompletedTask;
            };

            await Channel.BasicConsumeAsync(ReplyQueueName, true, consumer);
        }

        public async Task<Response> CallAsync(string routingKey, object? data = null, CancellationToken cancellationToken = default)
        {
            if (Channel is null)
            {
                throw new InvalidOperationException();
            }

            string correlationId = Guid.NewGuid().ToString();
            var props = new BasicProperties
            {
                CorrelationId = correlationId,
                ReplyTo = ReplyQueueName
            };

            var tcs = new TaskCompletionSource<Response>(TaskCreationOptions.RunContinuationsAsynchronously);
            CallbackMapper.TryAdd(correlationId, tcs);

            var request = data switch
            {
                null => null,
                string str => new Request(str),
                byte[] bytes => new Request(bytes),
                Request req => req,
                DataTransfer dat => dat,
                _ => new DataTransfer(data)
            };

            await Channel.BasicPublishAsync(
                exchange: string.Empty,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: props,
                body: request?.Data,
                cancellationToken: CancellationToken.None
            );

            using CancellationTokenRegistration ctr =
                cancellationToken.Register(() =>
                {
                    CallbackMapper.TryRemove(correlationId, out _);
                    tcs.SetCanceled();
                });

            return await tcs.Task;
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
