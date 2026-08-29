namespace WebAPI.Models
{
    public sealed class ErrorResponse
    {
        public int StatusCode { get; init; }
        public string Message { get; init; } = string.Empty;
        public string ErrorCode { get; init; } = string.Empty;
        public IReadOnlyDictionary<string, string[]>? Errors { get; init; }
        public DateTime Timestamp { get; init; } = DateTime.UtcNow;
    }
}
