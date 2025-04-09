using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Enums
{
    public enum AccountStats : byte
    {
        Active = 1,       //customer by default && Pharmacy owner after accepting the request (all)
        Suspended = 2,   //customer for suspicious activity && Pharmacy owner for suspicious activity (all)
        Pending = 4,    // pending for pharmacy owner to accept the request (Just Pharmacy)
        Banned = 5     //customer for not following the rules && Pharmacy owner for not following the rules (all)
    }
   
}
