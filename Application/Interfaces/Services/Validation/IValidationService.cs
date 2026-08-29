namespace Application.Interfaces.Services.Validation
{
    public interface IValidationService
    {
        Task ValidateAsync<T>(T instance, CancellationToken cancellationToken = default);
    }
}
