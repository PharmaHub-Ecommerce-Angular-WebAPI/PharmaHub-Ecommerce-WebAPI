using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PharmaHub.Service.UserHandler; 

public class SmsService : ISmsService
{
    private readonly IConfiguration _config;
    private readonly ILogger<SmsService> _logger;

    private static readonly Dictionary<string, (string Code, DateTime Expiry)> _phoneVerificationCodes
        = new();
    public SmsService(IConfiguration config , ILogger<SmsService> logger)
    {
        _config = config;
        _logger = logger;
        var sid = _config["Twilio:AccountSid"];
        var token = _config["Twilio:AuthToken"];

        _logger.LogInformation($"Twilio SID: {sid}");
        _logger.LogInformation($"Twilio AuthToken: {token}");

        TwilioClient.Init(sid, token);

    }

    public async Task SendSmsAsync(string toPhone, string message)
    {
        await MessageResource.CreateAsync(
            to: new Twilio.Types.PhoneNumber(toPhone),
            from: new Twilio.Types.PhoneNumber(_config["Twilio:PhoneNumber"]),
            body: message);
    }
    public async Task SendVerificationCodeAsync(string phoneNumber)
    {
        try
        {
            var code = new Random().Next(100000, 999999).ToString();

          
            _phoneVerificationCodes[phoneNumber] = (code, DateTime.UtcNow.AddMinutes(5));

          
            var accountSid = _config["Twilio:AccountSid"];
            var authToken = _config["Twilio:AuthToken"];
            var fromPhone = _config["Twilio:FromPhoneNumber"];

            TwilioClient.Init(accountSid, authToken);

            var message = await MessageResource.CreateAsync(
          body: $"Your verification code is: {code}",
          from: new Twilio.Types.PhoneNumber(fromPhone),
          to: new Twilio.Types.PhoneNumber(phoneNumber)
      );

            _logger.LogInformation($"Verification code {code} sent to {phoneNumber}");
        }
        catch (Exception ex)
        {
            _logger.LogError($"Failed to send verification code to {phoneNumber}: {ex.Message}");
            throw;
        }

    }
    public bool VerifyPhoneCode(string phoneNumber, string code)
    {
        if (_phoneVerificationCodes.TryGetValue(phoneNumber, out var stored))
        {
            var (storedCode, expiry) = stored;

            if (storedCode == code && expiry > DateTime.UtcNow)
            {
                return true;
            }
        }
        return false;
    }
}
