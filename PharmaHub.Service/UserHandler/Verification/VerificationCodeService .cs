using Microsoft.Extensions.Caching.Memory;

namespace PharmaHub.Service.UserHandler.Verification;

public class VerificationCodeService : IVerificationCodeService
{
    private readonly IMemoryCache _memoryCache;

    private readonly Dictionary<string, bool> _verifiedEmails = new();
    public VerificationCodeService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }

    private readonly TimeSpan _codeExpiration = TimeSpan.FromMinutes(10);
    private readonly TimeSpan _resendCooldown = TimeSpan.FromMinutes(1);
    private readonly int _maxAttempts = 3;
    private readonly TimeSpan _attemptsWindow = TimeSpan.FromHours(1);

    public string GenerateAndStoreCode(string email)
    {
        var code = new Random().Next(100000, 999999).ToString();

        var expiration = DateTime.UtcNow.Add(_codeExpiration);
        var data = (Code: code, Expiration: expiration, Attempts: 0, LastResendTime: DateTime.UtcNow);

        _memoryCache.Set(email.ToLower(), data, _codeExpiration);
        return code;
    }


    public string VerifyCode(string email, string inputCode)
    {
        if (_memoryCache.TryGetValue<(string Code, DateTime Expiration, int Attempts, DateTime LastResendTime)>(email.ToLower(), out var storedData))
        {
            if (storedData.Expiration < DateTime.UtcNow)
            {
                _memoryCache.Remove(email.ToLower());
                return "Expired";
            }

            if (storedData.Code == inputCode)
            {
               
                _verifiedEmails[email] = true;
             
                //_memoryCache.Remove(email.ToLower());
                Console.WriteLine($"Email verified: {email}");
                return "Valid";
            }

            storedData.Attempts++;
            if (storedData.Attempts >= _maxAttempts)
            {
                _memoryCache.Remove(email.ToLower());
            }
            else
            {
                _memoryCache.Set(email.ToLower(), storedData, storedData.Expiration - DateTime.UtcNow);

                Console.WriteLine($"Cache set for {email} with expiration at {storedData.Expiration}");
            }

            return "Invalid";
        }

        return "Invalid";
    }



    public void ClearCode(string email)
    {
        _memoryCache.Remove(email.ToLower());
    }

    public bool HasTooManyAttempts(string email)
    {
        if (_memoryCache.TryGetValue<(string Code, DateTime Expiration, int Attempts, DateTime LastResendTime)>(email.ToLower(), out var entry))
        {
            return entry.Attempts >= _maxAttempts &&
                   DateTime.UtcNow < entry.LastResendTime.Add(_attemptsWindow);
        }

        return false;
    }

    public bool LastResendWasTooRecent(string email)
    {
        if (_memoryCache.TryGetValue<(string Code, DateTime Expiration, int Attempts, DateTime LastResendTime)>(email.ToLower(), out var entry))
        {
            return DateTime.UtcNow < entry.LastResendTime.Add(_resendCooldown);
        }

        return false;
    }

    public int GetRemainingAttempts(string email)
    {
        if (_memoryCache.TryGetValue<(string Code, DateTime Expiration, int Attempts, DateTime LastResendTime)>(email.ToLower(), out var entry))
        {
            var attemptsRemaining = _maxAttempts - entry.Attempts;
            return attemptsRemaining > 0 ? attemptsRemaining : 0;
        }

        return _maxAttempts;
    }
    public bool IsVerified(string email)
    {
        Console.WriteLine($"Checking verification status for {email.ToLower()}");

        // Log the dictionary status
        Console.WriteLine($"Dictionary status for {email.ToLower()}: {_verifiedEmails.ContainsKey(email.ToLower())} = {_verifiedEmails.GetValueOrDefault(email.ToLower())}");

        // Check if the email is in the dictionary and if it's verified
        if (_verifiedEmails.TryGetValue(email.ToLower(), out bool isVerified))
        {
            Console.WriteLine($"Email found in dictionary, verified: {isVerified}");
            return isVerified;
        }

        // If not found in the dictionary, check the cache (optional)
        if (_memoryCache.TryGetValue<(string Code, DateTime Expiration, int Attempts, DateTime LastResendTime)>(email.ToLower(), out var storedData))
        {
            Console.WriteLine($"Email found in cache, code: {storedData.Code}, expiration: {storedData.Expiration}");
            return storedData.Expiration >= DateTime.UtcNow; // You can add more logic based on your needs
        }

        return false;
    }

}
