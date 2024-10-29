namespace PowerPlant.Api.IntegrationTests.Models;

public class ValidationErrorResponse
{
    public string Type { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int Status { get; set; }
    public string TraceId { get; set; } = null!;
    public Dictionary<string, List<string>> ErrorDetails { get; set; } = null!;
}