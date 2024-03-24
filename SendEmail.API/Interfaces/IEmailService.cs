namespace SendEmail.Infrastructure.Interfaces;

public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, string[] attachments = null);
}