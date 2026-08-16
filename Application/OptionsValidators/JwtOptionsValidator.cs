using Application.Models.Identity;
using Microsoft.Extensions.Options;

namespace Application.OptionsValidators
{
    public sealed class JwtOptionsValidator : IValidateOptions<JwtOptions>
    {
        public ValidateOptionsResult Validate(string? name, JwtOptions options)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(options.SecretKey))
            {
                errors.Add("JwtOptions:SecretKey is not configured.");
            }
            else if (options.SecretKey.Length < 32)
            {
                errors.Add(
                    "JwtOptions:SecretKey must contain at least 32 characters.");
            }

            if (string.IsNullOrWhiteSpace(options.Issuer))
            {
                errors.Add("JwtOptions:Issuer is not configured.");
            }

            if (string.IsNullOrWhiteSpace(options.Audience))
            {
                errors.Add("JwtOptions:Audience is not configured.");
            }

            if (options.AccessTokenLifetimeMinutes <= 0)
            {
                errors.Add(
                    "JwtOptions:AccessTokenLifetimeMinutes must be greater than 0.");
            }

            return errors.Count > 0
                ? ValidateOptionsResult.Fail(errors)
                : ValidateOptionsResult.Success;
        }
    }
}
