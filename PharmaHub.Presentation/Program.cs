
using PharmaHub.DAL.Repositories.GenericRepository;
using PharmaHub.DAL.Repositories.GenericRepositoryl;
using PharmaHub.DAL.Repositories.SuggestedMedicin;
using PharmaHub.DAL.Repositories.UnitOfWork;
using PharmaHub.DALRepositories;

namespace PharmaHub.Presentation;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        // Register Generic Repository
        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        // Register Specific Repositories
        builder.Services.AddScoped<ISuggestedMedicineRepository, SuggestedMedicineRepository>();

        // Register Unit of Work
        builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
        // Add services to the container.

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

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
