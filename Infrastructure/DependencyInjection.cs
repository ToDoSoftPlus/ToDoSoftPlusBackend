using Application.Interfaces.Repositories;
using Application.Interfaces.UnitOfWork;
using Infrastructure.DbContext;
using Infrastructure.Repositories;
using Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            services.AddScoped<IToDoSubItemRepository, ToDoSubItemRepository>();
            services.AddScoped<IToDoListRepository, ToDoListRepository>();

            return services;
        }
    }
}
