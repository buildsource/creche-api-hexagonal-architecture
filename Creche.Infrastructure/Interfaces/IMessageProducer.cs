namespace Creche.Infrastructure.Interfaces;

public interface IMessageProducer
{
    Task SendAsync(string message);
}