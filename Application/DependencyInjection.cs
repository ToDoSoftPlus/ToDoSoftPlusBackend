using Application.Interfaces.Services;
using Application.Services.EF;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var currentAssembly = typeof(DependencyInjection).Assembly;

            //Dependency all mapping profiles in current assembly
            services.AddAutoMapper(cfg => { }, currentAssembly);

            services.AddScoped<IToDoItemService, ToDoItemService>();

            return services;
        }
    }
}
