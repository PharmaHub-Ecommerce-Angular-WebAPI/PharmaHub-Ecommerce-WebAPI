using PharmaHub.DTOs.Payment;

namespace PharmaHub.Service.Payment;

public  interface IPaymentService
{
     Task<PaymentResult> ProcessPayment(string paymentToken, byte amount = 20);
}
