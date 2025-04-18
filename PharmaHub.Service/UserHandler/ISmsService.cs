namespace PharmaHub.Service.UserHandler;

public interface ISmsService
{
    Task SendSmsAsync(string toPhone, string message);
    Task SendVerificationCodeAsync(string phoneNumber);
    bool VerifyPhoneCode(string phoneNumber, string code);
}
