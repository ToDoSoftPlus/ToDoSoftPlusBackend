using Application.Interfaces.Services.EF;
using Application.Models.Identity;
using Application.OptionsValidators;
using Application.Services.EF;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var currentAssembly = typeof(DependencyInjection).Assembly;

            //Dependency all mapping profiles in current assembly
            services.AddAutoMapper(cfg => { }, currentAssembly);

            //Dependency all validators in current assembly
            services.AddValidatorsFromAssembly(currentAssembly);

            //Enable automatic validation of models using FluentValidation
            services.AddFluentValidationAutoValidation();

            services
                .AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateOnStart();

            services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

            services.AddScoped<IToDoItemService, ToDoItemService>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<IToDoSubItemService, ToDoSubItemService>();

            return services;
        }
    }
}
