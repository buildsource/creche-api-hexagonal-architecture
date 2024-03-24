using SendEmail.Infrastructure.Interfaces;
using System.Net.Mail;

namespace Creche.Infrastructure.Email;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly string _smtpServer;
    private readonly int _smtpPort;
    private readonly string _fromEmail;
    private readonly string _emailPassword;

    public EmailService(ILogger<EmailService> logger, string smtpServer, int smtpPort, string fromEmail, string emailPassword)
    {
        _logger = logger;
        _smtpServer = smtpServer;
        _smtpPort = smtpPort;
        _fromEmail = fromEmail;
        _emailPassword = emailPassword;
    }

    public async Task SendEmailAsync(string to, string subject, string body, string[] attachments = null)
    {
        using var mailMessage = new MailMessage(_fromEmail, to)
        {
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        if (attachments != null)
            foreach (var attachmentPath in attachments)
            {
                mailMessage.Attachments.Add(new Attachment(attachmentPath));
            }

        using var smtpClient = new SmtpClient(_smtpServer)
        {
            Port = _smtpPort,
            Credentials = new System.Net.NetworkCredential(_fromEmail, _emailPassword),
            EnableSsl = true,
        };

        try
        {

            _logger.LogInformation("Email sending started.");

            await smtpClient.SendMailAsync(mailMessage);

            _logger.LogInformation("Email sending finished.");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro to send email: {ex.Message}");
            throw;
        }
    }
}