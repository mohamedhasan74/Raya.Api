
using Microsoft.EntityFrameworkCore;
using Raya.Api.Helpers;
using Raya.Core.Interfaces;
using Raya.Repository;
using Raya.Repository.Data;

namespace Raya.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.ConfigrationDataBase();
            builder.ConfigrationServices();
            var app = builder.Build();
            using(var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var _context = serviceProvider.GetRequiredService<AppDbContext>();
                var _loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
                try
                {
                    await _context.Database.MigrateAsync();
                    await AppDbContextSeed.ProductSeedAsync(_context);
                }
                catch(Exception ex) 
                {
                    var logger = _loggerFactory.CreateLogger(typeof(Program));
                    logger.LogError(ex, ex.Message);
                }
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseCors(options => options.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}