using Application.Interfaces.Services;
using Application.Services.EF;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddScoped<IToDoItemService, ToDoItemService>();

            return services;
        }
    }
}
