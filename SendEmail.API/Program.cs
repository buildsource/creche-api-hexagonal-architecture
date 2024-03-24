using Creche.Infrastructure.Email;
using SendEmail.API.Messaging;
using SendEmail.API.Options;
using SendEmail.Infrastructure.Interfaces;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddSingleton<IEmailService>(serviceProvider =>
{
    var logger = serviceProvider.GetRequiredService<ILogger<EmailService>>();
    var emailOptions = builder.Configuration.GetSection("Email").Get<EmailOptions>();
    return new EmailService(
        logger,
        emailOptions.SmtpServer,
        emailOptions.SmtpPort,
        emailOptions.FromEmail,
        emailOptions.EmailPassword);
});

var app = builder.Build();

// Iniciar a escuta do RabbitMQ
var rabbitMQOptions = builder.Configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
var emailService = app.Services.GetRequiredService<IEmailService>();
var logger = app.Services.GetRequiredService<ILogger<RabbitMQMessageConsumer>>();

var messageConsumer = new RabbitMQMessageConsumer(logger, rabbitMQOptions.Hostname, rabbitMQOptions.QueueName, emailService);

messageConsumer.StartConsuming();


app.UseHttpsRedirection();

app.MapGet("/", () => "Welcome to running ASP.NET Core Minimal API on AWS Lambda");

app.Run();