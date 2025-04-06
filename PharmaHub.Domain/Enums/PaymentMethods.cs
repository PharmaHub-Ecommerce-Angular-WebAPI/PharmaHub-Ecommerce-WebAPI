using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Enums
{
    public enum PaymentMethods: byte
    {
        CashOnDelivery,
        CreditCard,
        PayPal,
        ApplePay,
        AmazonPay
    }
  
}
