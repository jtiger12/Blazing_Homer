namespace HomerBlazor.Core.Models;

public class ServiceCardData
{
    public ServiceStatus Status { get; set; } = ServiceStatus.Unknown;
    public string? StatusMessage { get; set; }
    public Dictionary<string, object> Data { get; set; } = new();
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public TimeSpan? NextRefresh { get; set; }
    public string? ErrorMessage { get; set; }
    public bool HasError => !string.IsNullOrEmpty(ErrorMessage);
    public DateTime LastChecked { get; set; } = DateTime.UtcNow;
    public TimeSpan? ResponseTime { get; set; }
}

public enum ServiceStatus
{
    Unknown,
    Online,
    Offline,
    Warning,
    Error,
    Maintenance
}
