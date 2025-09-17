using YamlDotNet.Serialization;

namespace HomerBlazor.Core.Models;

// Note: ServiceStatus enum and ServiceCardData class are already defined in ServiceCardData.cs
// This file can be removed or used for additional status-related extensions

public static class ServiceStatusExtensions
{
    public static bool IsOnline(this ServiceStatus status)
    {
        return status == ServiceStatus.Online;
    }

    public static string GetDisplayText(this ServiceStatus status)
    {
        return status switch
        {
            ServiceStatus.Online => "Online",
            ServiceStatus.Offline => "Offline", 
            ServiceStatus.Warning => "Warning",
            ServiceStatus.Error => "Error",
            ServiceStatus.Maintenance => "Maintenance",
            _ => "Unknown"
        };
    }

    public static string GetCssClass(this ServiceStatus status)
    {
        return status switch
        {
            ServiceStatus.Online => "status-online",
            ServiceStatus.Offline => "status-offline",
            ServiceStatus.Warning => "status-warning", 
            ServiceStatus.Error => "status-error",
            ServiceStatus.Maintenance => "status-maintenance",
            _ => "status-unknown"
        };
    }
}