using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaHub.DAL.IdentityMapping;
using PharmaHub.Domain.Entities;

namespace PharmaHub.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<PackagesComponent> PackagesComponents { get; set; }
        public DbSet<SuggestedMedicine> SuggestedMedicines { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }


    }
}
