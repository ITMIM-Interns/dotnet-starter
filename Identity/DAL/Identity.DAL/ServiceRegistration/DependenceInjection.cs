using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Repositories;
using Identity.DAL.Data;
using Identity.DAL.Implementations.Externals.Emails;
using Identity.DAL.Implementations.Externals.Files;
using Identity.DAL.Internals;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.DAL.ServiceRegistration
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services,string connectionString)
        {
            //---------------------------Internals---------------------------------------------------------------------
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUserVerificationRepository, UserVerificationRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            //-----------------------------External services-------------------------------------------------------
            services.AddScoped<IFileService, AmazonS3Service>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            return services;
        }
    }
}
