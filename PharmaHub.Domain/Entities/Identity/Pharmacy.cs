using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Entities.Identity
{
    public class Pharmacy : User
    {

        [Required] public string PharmacyName { get; set; } // Will use Default Identity UserName for customer Full name. 

        public bool AllowCreditCard { get; set; } // If the pharmacy accept credit card (true) or not.
        public string CreditCardNumber { get; set; }



        [Range(0, 24, ErrorMessage = "Time must be from 0 to 24.")]
        public byte OpenTime { get; set; } = 0;  //If 0 then 24/7

        [Range(0, 24, ErrorMessage = "Time must be from 0 to 24.")]
        public byte CloseTime { get; set; } = 0;  //If 0 then 24/7



        public string FormalPapersURL { get; set; }

       public string LogoURL { get; set; }

    }
}
