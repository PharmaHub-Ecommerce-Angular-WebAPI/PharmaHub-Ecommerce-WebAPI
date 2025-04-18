﻿using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.Domain.Entities
{
    public class FavoriteProduct
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public string CustomerId { get; set; }
        public Customer Customer { get; set; }

    }
}
