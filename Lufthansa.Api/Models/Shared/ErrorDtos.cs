using System.Text.Json.Serialization;

namespace Lufthansa.Api.Models.Shared;

public class ProcessingErrorResponse
{
    public ProcessingErrors? ProcessingErrors { get; set; }
}

public class ProcessingErrors
{
    public ProcessingError? ProcessingError { get; set; }
}

public class ProcessingError
{
    [JsonPropertyName("@RetryIndicator")] public bool RetryIndicator { get; set; }

    public string? Type { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    [JsonPropertyName("@InfoURL")] public string? InfoUrl { get; set; }
}