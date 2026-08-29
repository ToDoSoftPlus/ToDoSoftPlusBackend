using Application.Interfaces.Services.EF;
using Application.Interfaces.Services.Validation;
using Application.Models.Identity;
using Application.OptionsValidators;
using Application.Services.EF;
using Application.Services.Validation;
using FluentValidation;
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

            services
                .AddOptions<JwtOptions>()
                .BindConfiguration(JwtOptions.SectionName)
                .ValidateOnStart();

            services.AddSingleton<IValidateOptions<JwtOptions>, JwtOptionsValidator>();

            services.AddScoped<IToDoItemService, ToDoItemService>();
            services.AddScoped<IToDoListService, ToDoListService>();
            services.AddScoped<IToDoSubItemService, ToDoSubItemService>();
            services.AddScoped<IValidationService, ValidationService>();

            return services;
        }
    }
}
