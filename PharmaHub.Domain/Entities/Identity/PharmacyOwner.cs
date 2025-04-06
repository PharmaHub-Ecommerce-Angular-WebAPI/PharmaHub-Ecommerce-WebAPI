using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Entities.Identity
{
    public class PharmacyOwner: User
    {
        public string CreditCard { get; set; }
        public bool AllowCreditCard { get; set; }

        [Range(0, 24, ErrorMessage = "Time must be from 1 to 24.")]
        public byte OpenTime { get; set; } = 0;//if 0 then 24/7

        [Range(0, 24, ErrorMessage = "Time must be from 1 to 24.")]
        public byte CloseTime { get; set; } = 0;//if 0 then 24/7
        public string FormalPapersURL { get; set; }

        public Governorates Country { get; set; }

        [Url]
        public string LogoURL { get; set; }

    }
}
