namespace PharmaHub.Service.UserHandler.Verification;

public interface IVerificationCodeService
{
    string GenerateAndStoreCode(string email);
    string VerifyCode(string email, string inputCode);
    void ClearCode(string email);
    bool HasTooManyAttempts(string email);
    bool LastResendWasTooRecent(string email);
    int GetRemainingAttempts(string email);
}