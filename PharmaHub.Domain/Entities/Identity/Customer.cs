using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmaHub.Domain.Entities.Identity
{
    public class Customer:User
    {
        //The Full Name Will put in the UserName in the IdentityUser 


        //relation
        public ICollection<FavoriteProduct> FavoriteProductsList { get; set; } = new List<FavoriteProduct>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
