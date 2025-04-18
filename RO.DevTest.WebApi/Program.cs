using Microsoft.EntityFrameworkCore;
using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence;
using RO.DevTest.Persistence.IoC;

namespace RO.DevTest.WebApi;
    
public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies();

        builder.Services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssemblies(
                typeof(ApplicationLayer).Assembly,
                typeof(Program).Assembly
            );
        });

        var app = builder.Build();

        if(app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
