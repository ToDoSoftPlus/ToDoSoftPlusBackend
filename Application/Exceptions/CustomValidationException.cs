namespace Application.Exceptions
{
    public class CustomValidationException : Exception
    {
        public IReadOnlyDictionary<string, string[]> Errors { get; }

        public CustomValidationException(IDictionary<string, string[]> errors) : base("One or more validation errors occurred.")
        {
            Errors = new Dictionary<string, string[]>(errors);
        }
    }
}
