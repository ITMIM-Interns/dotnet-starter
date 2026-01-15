using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MiniApp.BLL.Abstractions.Externals;
using MiniApp.BLL.Abstractions.Internals.Reads;
using MiniApp.BLL.Abstractions.Internals.UnitOfWork;
using MiniApp.BLL.Abstractions.Internals.Writes;
using MiniApp.DAL.Implementations.Externals.Emails;
using MiniApp.DAL.Implementations.Externals.Files;
using MiniApp.DAL.Implementations.Internals.Reads;
using MiniApp.DAL.Implementations.Internals.Writes;
using MiniApp.DAL.Implementations.UnitOfWork;
using MiniApp.DataAccess.Data;

namespace MiniApp.DAL.ServiceRegistration
{
    public static class DependenceInjection
    {
        public static IServiceCollection AddDALServices(this IServiceCollection services,string connectionString)
        {
            //---------------------------Internals---------------------------------------------------------------------
            services.AddScoped(typeof(IGenericReadRepository<,>), typeof(GenericReadRepository<,>));
            services.AddScoped(typeof(IGenericWriteRepository<,>), typeof(GenericWriteRepository<,>));
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IUserVerificationReadRepository, UserVerificationReadRepository>();
            services.AddScoped<IUserVerificationWriteRepository, UserVerificationWriteRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
            //-----------------------------External services-------------------------------------------------------
            services.AddScoped<IFileService, AmazonS3Service>();
            services.AddScoped<IEmailService, SmtpEmailService>();
            return services;
        }
    }
}
