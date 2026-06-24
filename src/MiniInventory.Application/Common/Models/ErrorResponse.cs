namespace MiniInventory.Application.Common.Models;

public class ErrorResponse
{
    public int Status { get; init; }
    public string Message { get; init; } = string.Empty;
    public IEnumerable<string>? Errors { get; init; }
}
