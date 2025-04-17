using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace PharmaHub.Service.Services;

public class SmsService : ISmsService
{
    private readonly IConfiguration _config;

    public SmsService(IConfiguration config)
    {
        _config = config;
        TwilioClient.Init(_config["Twilio:SID"], _config["Twilio:AuthToken"]);
    }

    public async Task SendSmsAsync(string toPhone, string message)
    {
        await MessageResource.CreateAsync(
            to: new Twilio.Types.PhoneNumber(toPhone),
            from: new Twilio.Types.PhoneNumber(_config["Twilio:PhoneNumber"]),
            body: message);
    }
}
