using HomerBlazor.Core.Models;

namespace HomerBlazor.Core.Interfaces;

public interface IConfigurationService
{
    Task<DashboardConfig> LoadConfigurationAsync(string? configPath = null);
    Task SaveConfigurationAsync(DashboardConfig config, string? configPath = null);
    Task<bool> ValidateConfigurationAsync(DashboardConfig config);
    Task<DashboardConfig> GetConfigurationAsync();
    event EventHandler<DashboardConfig>? ConfigurationChanged;
}
