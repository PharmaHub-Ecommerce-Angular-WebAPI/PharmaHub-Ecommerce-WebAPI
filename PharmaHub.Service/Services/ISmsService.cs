namespace PharmaHub.Service.Services;

public interface ISmsService
{
    Task SendSmsAsync(string toPhone, string message);
}
