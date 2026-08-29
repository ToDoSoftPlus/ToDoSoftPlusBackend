using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Exceptions
{
    public sealed class IdentityException : Exception
    {
        public IEnumerable<IdentityError> Errors { get; }
        public IdentityException(IEnumerable<IdentityError> errors) : base(string.Join(", ", errors.Select(e => e.Description)))
        {
            Errors = errors;
        }
    }
}
