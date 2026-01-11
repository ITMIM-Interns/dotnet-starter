using Microsoft.Extensions.DependencyInjection;
using MiniApp.BLL.Features.Queries.Users.GetById;
using MediatR;

namespace MiniApp.BLL.ServiceRegistration
{
    public static class BLLService
    {
        public static IServiceCollection AddBLLServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(GetByIdUserQueryHandler).Assembly);
            return services;
        }
    }
}
