using Microsoft.Extensions.Options;
using PharmaHub.DTOs.Payment;
using Stripe;

namespace PharmaHub.Service.Payment;

public class PaymentService
{
    private readonly string _secretKey;
    public PaymentService(IOptions<StripeSettings> stripeSettings)
    {
        _secretKey = stripeSettings.Value.SecretKey;
        StripeConfiguration.ApiKey = _secretKey;
    }
    public async Task<PaymentResult> ProcessPayment(string paymentToken, int amount = 20)
    {
        //  Configure Stripe
        StripeConfiguration.ApiKey = "sk_test_51R40K2K0i4ouTtOMJUqnSovmijOjGf5hVnxNY6FtCil3Dv5zx6kPwbaR7II6AMeNFlMZhVQAWqdFNR3zn59D7cDP001EwLibdN";

        // Create the payment charge options
        var options = new ChargeCreateOptions
        {
            Amount = 2000, // Payment amount in cents
            Currency = "usd",
            Description = "Order Payment",
            Source = paymentToken, // Stripe token from the client
        };

        //  Execute the payment
        var service = new ChargeService();
        try
        {
            var charge = await service.CreateAsync(options);

            // Return success if payment is successful
            return new PaymentResult
            {
                Success = charge.Status == "succeeded",
                ChargeId = charge.Id
            };
        }
        catch (Exception ex)
        {
           
            Console.WriteLine(ex.Message);
            return new PaymentResult { Success = false };
        }
    }

}

