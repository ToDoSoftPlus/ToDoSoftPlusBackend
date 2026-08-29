using Application.Exceptions;
using Application.Interfaces.Services.Validation;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Services.Validation
{
    public class ValidationService : IValidationService
    {
        private readonly IServiceProvider _serviceProvider;

        public ValidationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ValidateAsync<T>(T instance, CancellationToken cancellationToken = default)
        {
            var validator = _serviceProvider.GetService<IValidator<T>>();

            if (validator is null)
                return;

            var result = await validator.ValidateAsync(instance, cancellationToken);

            if (result.IsValid)
                return;

            var errors = result.Errors
                .GroupBy(x => x.PropertyName)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(x => x.ErrorMessage).ToArray());

            throw new CustomValidationException(errors);
        }
    }
}
