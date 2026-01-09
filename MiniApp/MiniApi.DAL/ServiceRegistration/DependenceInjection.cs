using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.DAL.Implementations.Internals.Reads;
using MiniApp.DAL.Implementations.Internals.Writes;
using MiniApp.DataAccess.Data;

namespace MiniApp.DAL.ServiceRegistration
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services,string connectionString)
        {
            services.AddScoped(typeof(IGenericReadRepository<,>), typeof(GenericReadRepository<,>));
            services.AddScoped(typeof(IGenericWriteRepository<,>), typeof(GenericWriteRepository<,>));
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}
