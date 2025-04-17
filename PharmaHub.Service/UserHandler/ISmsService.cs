namespace PharmaHub.Service.UserHandler;

public interface ISmsService
{
    Task SendSmsAsync(string toPhone, string message);
}
