using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MiniApp.BLL.Behaviors;
using MiniApp.BLL.Features.Commands.Users.Create;
using MiniApp.BLL.Features.Queries.Users.GetById;
using MiniApp.BLL.FluentValidations.Users;

namespace MiniApp.BLL.ServiceRegistration
{
    public static class BLLService
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetByIdUserQueryHandler).Assembly);
            services.AddValidatorsFromAssembly(typeof(CreateUserCommandHandler).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            return services;
        }
    }
}
