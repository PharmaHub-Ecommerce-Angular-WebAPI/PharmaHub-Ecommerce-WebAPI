using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities.Identity;
using PharmaHub.Domain.Entities.Identity.Contract;

namespace PharmaHub.Domain.Entities
{
    public class FavoriteProduct
    {
        public virtual Product Product { get; set; }

        public int ProductId { get; set; }
        public int UserId { get; set; } 
        public  IUser User { get; set; }



    }
}
