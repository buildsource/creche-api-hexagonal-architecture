using Creche.Infrastructure.Interfaces;
using RabbitMQ.Client;
using System.Text;

namespace Creche.Infrastructure.Messaging;

public class RabbitMQMessageProducer : IMessageProducer
{
    private readonly string _hostname;
    private readonly string _queueName;

    public RabbitMQMessageProducer(string hostname, string queueName)
    {
        _hostname = hostname;
        _queueName = queueName;
    }

    public async Task SendAsync(string message)
    {
        var factory = new ConnectionFactory() { HostName = _hostname };
        using (var connection = factory.CreateConnection())
        using (var channel = connection.CreateModel())
        {
            channel.QueueDeclare(queue: _queueName,
                                 durable: true,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var body = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                 routingKey: _queueName,
                                 basicProperties: null,
                                 body: body);
            await Task.CompletedTask;
        }
    }
}