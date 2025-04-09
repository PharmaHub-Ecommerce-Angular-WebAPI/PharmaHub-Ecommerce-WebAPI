﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.Domain.Entities
{
    public class FavoriteProduct
    {
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int CustomerId { get; set; } 
        public  Customer Customer { get; set; }

    }
}
