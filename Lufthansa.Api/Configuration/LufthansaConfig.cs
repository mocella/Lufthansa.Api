namespace Lufthansa.Api.Configuration;

public class LufthansaConfig
{
    private string? _baseUrl;

    public string? BaseUrl
    {
        get => string.IsNullOrWhiteSpace(_baseUrl) ? string.Empty : _baseUrl.TrimEnd('/') + "/";
        set => _baseUrl = value;
    }

    public string? ClientId { get; set; }
    public string? ClientSecret { get; set; }
}