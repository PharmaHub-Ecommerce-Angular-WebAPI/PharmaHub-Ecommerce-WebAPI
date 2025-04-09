using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PharmaHub.Domain.Enums;

namespace PharmaHub.Domain.Entities.Identity
{
    public class User: IdentityUser
    {

        public CountriesSupported Country { get; set; } = CountriesSupported.Egypt; // Default to Egypt
        public Governorates city { get; set; }

        [Required] public string Address { get; set; } 

        public AccountStats AccountStat { get; set; }

        public byte TrustScore { get; set; } // For reporting system (under 50 suspicious in 0 Banned)
    }
}
