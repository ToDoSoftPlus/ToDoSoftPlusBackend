using Application.Interfaces.Repositories;
using Application.Interfaces.Services.Identity;
using Application.Interfaces.UnitOfWork;
using Domain.Entities;
using Infrastructure.DbContext;
using Infrastructure.Identity;
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
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Connection string {"DefaultConnection"} is missing in appsettings.json or environment variables.");
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            //Identity
            services.AddIdentityCore<ApplicationUser>(options => 
            {
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 1;
            })
                .AddRoles<ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IToDoItemRepository, ToDoItemRepository>();
            services.AddScoped<IToDoSubItemRepository, ToDoSubItemRepository>();
            services.AddScoped<IToDoListRepository, ToDoListRepository>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IIdentityService, IdentityService>();

            return services;
        }
    }
}
