using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SendEmail.API.DTOs;
using SendEmail.Infrastructure.Interfaces;
using System.Text;
using System.Text.Json;

namespace SendEmail.API.Messaging;

public class RabbitMQMessageConsumer : IMessageConsumer
{
    private readonly ILogger<RabbitMQMessageConsumer> _logger;
    private readonly string _hostname;
    private readonly string _queueName;
    private readonly IEmailService _emailService;

    public event EventHandler<MessageReceivedEventArgs> OnMessageReceived;

    public RabbitMQMessageConsumer(
        ILogger<RabbitMQMessageConsumer> logger,
        string hostname, 
        string queueName, 
        IEmailService emailService
    )
    {
        _logger = logger;
        _hostname = hostname;
        _queueName = queueName;
        _emailService = emailService;
    }

    public void StartConsuming()
    {
        var factory = new ConnectionFactory() { HostName = _hostname };
        var connection = factory.CreateConnection();
        var channel = connection.CreateModel();

        channel.QueueDeclare(queue: _queueName,
                             durable: true,
                             exclusive: false,
                             autoDelete: false,
                             arguments: null);

        var consumer = new EventingBasicConsumer(channel);
        consumer.Received += async (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var jsonString = Encoding.UTF8.GetString(body);
            _logger.LogInformation($"Received message: {jsonString}");

            try
            {
                var email = JsonSerializer.Deserialize<EmailDto>(jsonString);
                if (email != null)
                {
                    var subject = $"New message to {email.Nome}";
                    var bodyMessage = $"Olá {email.Nome}, \n\n{email.Message}.";
                    await _emailService.SendEmailAsync(email.Email, subject, bodyMessage);
                }
            }
            catch (JsonException ex)
            {
                _logger.LogError($"Error when deserving the message: {ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro to send email: {ex.Message}");
            }
        };

        channel.BasicConsume(queue: _queueName,
        autoAck: true,
        consumer: consumer);

        _logger.LogInformation("Consumer Started...");
    }
}

public class MessageReceivedEventArgs : EventArgs
{
    public string Message { get; set; }
}