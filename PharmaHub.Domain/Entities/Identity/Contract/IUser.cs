using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Entities.Identity.Contract
{
    public interface IUser
    {
        public string PharmacyName { get; set; }
        public bool AllowCreditCard { get; set; }
        public string CreditCardNumber { get; set; }
        public byte OpenTime { get; set; }  
        public byte CloseTime { get; set; }  
        public string FormalPapersURL { get; set; }
        public CountriesSupported Country { get; set; }
        public Governorates city { get; set; }
        public string LogoURL { get; set; }
    }
}
