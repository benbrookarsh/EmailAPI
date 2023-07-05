namespace API.Models;

public class ErrorResponse
{
    public string Message { get; set; }
    public DateTime LastValidRequestTime { get; set; }
}