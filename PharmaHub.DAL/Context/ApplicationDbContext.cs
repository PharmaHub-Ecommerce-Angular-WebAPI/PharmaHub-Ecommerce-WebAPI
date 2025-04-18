using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Entities.Identity;

namespace PharmaHub.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        //check if the below code is Full
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductOrders { get; set; }
        public DbSet<PackagesComponent> PackagesComponents { get; set; }
        public DbSet<SuggestedMedicine> SuggestedMedicines { get; set; }
        public DbSet<User> Users { get; set; } ////////////////// Is it accurate ?/////////////
        public DbSet<Pharmacy> Pharmacies { get; set; }
        public DbSet<FavoriteProduct> FavoriteProducts { get; set; }




        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);


            //Seeds Roles 

            var adminRoleId = Guid.NewGuid().ToString();
            modelBuilder.Entity<IdentityRole>().HasData(
               new IdentityRole
               {
                   Id = Guid.NewGuid().ToString(), 
                   Name = "Customer",
                   NormalizedName = "CUSTOMER"
               },
               new IdentityRole
               {
                   Id = Guid.NewGuid().ToString(), 
                   Name = "Pharmacy",
                   NormalizedName = "PHARMACY"
               },
               new IdentityRole
               {
                   Id = adminRoleId, 
                   Name = "Admin",
                   NormalizedName = "ADMIN"
               }
            );


            // Seeds Admin User

            var hasher = new PasswordHasher<User>();
            var adminEmail = "admin@example.com";
            var adminId = Guid.NewGuid().ToString();
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = adminId,
                    UserName = adminEmail,
                    NormalizedUserName = adminEmail.ToUpper(),
                    Email = adminEmail,
                    NormalizedEmail = adminEmail.ToUpper(),
                    EmailConfirmed = true,
                    PasswordHash = "AQAAAAIAAYagAAAAEFnupFy23xRw9iVXAg5GjG/XYytPSD0EnHT4OLSctd+IC/rDBqUPFPbBLyEztH7fwg==", //Hash for Admin@12345
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Address = "Admin HQ",
                    PhoneNumber = "+200000000000"
                }
            );

            modelBuilder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string>
                {
                    UserId = adminId,
                    RoleId = adminRoleId
                }
            );




        }

    }
}
