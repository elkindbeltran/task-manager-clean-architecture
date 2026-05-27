namespace TaskManagerCleanArchitecture.API.Contracts.Errors;

public class ApiErrorResponse
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
    public object? Errors { get; set; }
    public string? TraceId { get; set; }
}