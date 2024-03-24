namespace SendEmail.API.Options;

public class EmailOptions
{
    public string SmtpServer { get; set; }
    public int SmtpPort { get; set; }
    public string FromEmail { get; set; }
    public string EmailPassword { get; set; }
}