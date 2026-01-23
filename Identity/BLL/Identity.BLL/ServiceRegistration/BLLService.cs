using FluentValidation;
using Identity.BLL.Abstractions.Externals;
using Identity.BLL.Abstractions.Internals.Services;
using Identity.BLL.FluentValidations.Users;
using Identity.BLL.ServiceImplementation;
using Microsoft.Extensions.DependencyInjection;

namespace Identity.BLL.ServiceRegistration
{
    public static class BLLService
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(CreateUserDtoValidator).Assembly);
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITokenService,TokenService>();
            return services;
        }
    }
}
