
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using PharmaHub.Business.Managers;
using PharmaHub.DAL.Context;
using PharmaHub.DAL.Repositories;
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.PackagesComponent;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.DALRepositories;
using PharmaHub.Domain.Entities;
using PharmaHub.Domain.Infrastructure;
using PharmaHub.Service.JWT_Handler;

namespace PharmaHub.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        #region Injection Dependence Configuration
        // Register Generic Repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Register Specific Repositories
        builder.Services.AddScoped<ISuggestedMedicineRepository, SuggestedMedicineRepository>();

        // Register Unit of Product
        builder.Services.AddScoped<IProductRepository, ProductRepository>();

        // Register Unit of Order
        builder.Services.AddScoped<IOrderRepository, OrderRepository>();

        // Register Unit of PackagesComponent
        builder.Services.AddScoped<IPackagesComponentRepository, PackagesComponentRepository>();

        // Register Unit of Favorite Product
        builder.Services.AddScoped<IFavoriteProductRepository, FavoriteProductRepository>();

        // Register Unit of Work
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); 
        #endregion

        // Add services to the container.

        #region Make_connectionstring

        // Add services to the container.
        var ConnectionString = builder.Configuration.GetConnectionString("mahdyConnectionString");
        builder.Services.AddDbContext<ApplicationDbContext>(
            options =>
            {
                options
                    .UseSqlServer(ConnectionString) //whyyyyy must Domain
                    .LogTo(Console.WriteLine, LogLevel.Warning);
            }
        );

        #endregion


        #region Configration_identity

        builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = true;
            //options.Password.RequiredLength = 6;
            //options.Password.RequireNonAlphanumeric = false;
            //options.Password.RequireUppercase = false;
            //options.Password.RequireLowercase = false;
            options.User.RequireUniqueEmail = true; // Ensure unique email addresses
        })
            //.AddRoles<IdentityRole>()  // Enable role-based authorization
            .AddEntityFrameworkStores<ApplicationDbContext>() 
            .AddDefaultTokenProviders();

        #endregion

        #region JWT_Configrations
        /*
         * 
         * 
         * 
        // Identity with Roles
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
         * */

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateLifetime = true,

                ValidateIssuer = true,
                ValidIssuer= builder.Configuration["JwtSettings:Issuer"],

                ValidateAudience = true,
                ValidAudience = builder.Configuration["JwtSettings:Audience"],

                ValidateIssuerSigningKey = true,

                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
            };
        });
        builder.Services.AddAuthorization();
        builder.Services.AddScoped(sp => new JwtTokenService(builder.Configuration["JwtSettings: Key"], builder.Configuration["JwtSettings:Issuer"], builder.Configuration["JwtSettings:Audience"], sp.GetRequiredService<UserManager<IdentityUser>>()));


        #endregion


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
