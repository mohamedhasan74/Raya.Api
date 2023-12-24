using Microsoft.EntityFrameworkCore;
using Raya.Api.Helpers;
using Raya.Core.Interfaces;
using Raya.Repository;
using Raya.Repository.Data;

namespace Raya.Api
{
    public static class InfrastractureExtintions
    {
        public static WebApplicationBuilder ConfigrationDataBase(this WebApplicationBuilder builder)
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            return builder;
        }
        public static WebApplicationBuilder ConfigrationServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.AddCors();
            return builder;
        }
    }
}
