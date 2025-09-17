using HomerBlazor.Core.Interfaces;
using HomerBlazor.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace HomerBlazor.Core.Services;

public class ConfigurationService : IConfigurationService
{
    private readonly ILogger<ConfigurationService> _logger;
    private readonly string _configPath;
    private readonly IDeserializer _yamlDeserializer;
    private readonly ISerializer _yamlSerializer;
    private DashboardConfig? _cachedConfig;
    private FileSystemWatcher? _fileWatcher;

    public event EventHandler<DashboardConfig>? ConfigurationChanged;

    public ConfigurationService(ILogger<ConfigurationService> logger, IOptions<ConfigurationOptions> options)
    {
        _logger = logger;
        _configPath = options.Value.ConfigPath ?? "config/config.yml";
        
        _yamlDeserializer = new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .IgnoreUnmatchedProperties()
            .Build();

        _yamlSerializer = new SerializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();

        SetupFileWatcher();
    }

    public async Task<DashboardConfig> LoadConfigurationAsync(string? configPath = null)
    {
        var path = configPath ?? _configPath;
        
        try
        {
            if (!File.Exists(path))
            {
                _logger.LogWarning("Configuration file not found at {Path}, creating default configuration", path);
                var defaultConfig = CreateDefaultConfiguration();
                await SaveConfigurationAsync(defaultConfig, path);
                return defaultConfig;
            }

            var yamlContent = await File.ReadAllTextAsync(path);
            var config = _yamlDeserializer.Deserialize<DashboardConfig>(yamlContent);
            
            _cachedConfig = config;
            _logger.LogInformation("Configuration loaded successfully from {Path}", path);
            
            return config;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load configuration from {Path}", path);
            throw new InvalidOperationException($"Failed to load configuration: {ex.Message}", ex);
        }
    }

    public async Task SaveConfigurationAsync(DashboardConfig config, string? configPath = null)
    {
        var path = configPath ?? _configPath;
        
        try
        {
            var directory = Path.GetDirectoryName(path);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            var yamlContent = _yamlSerializer.Serialize(config);
            await File.WriteAllTextAsync(path, yamlContent);
            
            _cachedConfig = config;
            _logger.LogInformation("Configuration saved successfully to {Path}", path);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to save configuration to {Path}", path);
            throw new InvalidOperationException($"Failed to save configuration: {ex.Message}", ex);
        }
    }

    public async Task<bool> ValidateConfigurationAsync(DashboardConfig config)
    {
        try
        {
            // Basic validation
            if (string.IsNullOrWhiteSpace(config.Title))
            {
                _logger.LogWarning("Configuration validation failed: Title is required");
                return false;
            }

            if (config.Services == null || !config.Services.Any())
            {
                _logger.LogWarning("Configuration validation failed: At least one service group is required");
                return false;
            }

            foreach (var group in config.Services)
            {
                if (string.IsNullOrWhiteSpace(group.Name))
                {
                    _logger.LogWarning("Configuration validation failed: Service group name is required");
                    return false;
                }

                foreach (var item in group.Items)
                {
                    if (string.IsNullOrWhiteSpace(item.Name))
                    {
                        _logger.LogWarning("Configuration validation failed: Service item name is required");
                        return false;
                    }
                }
            }

            _logger.LogDebug("Configuration validation passed");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Configuration validation failed with exception");
            return false;
        }
    }

    public async Task<DashboardConfig> GetConfigurationAsync()
    {
        if (_cachedConfig == null)
        {
            _cachedConfig = await LoadConfigurationAsync();
        }
        
        return _cachedConfig;
    }

    private void SetupFileWatcher()
    {
        try
        {
            var directory = Path.GetDirectoryName(_configPath);
            var fileName = Path.GetFileName(_configPath);
            
            if (string.IsNullOrEmpty(directory))
            {
                directory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            _fileWatcher = new FileSystemWatcher(directory, fileName)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size,
                EnableRaisingEvents = true
            };

            _fileWatcher.Changed += OnConfigFileChanged;
            _logger.LogDebug("File watcher setup for configuration file: {Path}", _configPath);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to setup file watcher for configuration file");
        }
    }

    private async void OnConfigFileChanged(object sender, FileSystemEventArgs e)
    {
        try
        {
            // Debounce file changes
            await Task.Delay(500);
            
            var newConfig = await LoadConfigurationAsync();
            ConfigurationChanged?.Invoke(this, newConfig);
            
            _logger.LogInformation("Configuration file changed and reloaded");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to reload configuration after file change");
        }
    }

    private static DashboardConfig CreateDefaultConfiguration()
    {
        return new DashboardConfig
        {
            Title = "Homer Dashboard",
            Subtitle = "Welcome to your new dashboard!",
            Theme = "default",
            Header = true,
            Columns = "auto",
            Services = new List<ServiceGroup>
            {
                new()
                {
                    Name = "Applications",
                    Icon = "fas fa-cloud",
                    Items = new List<ServiceItem>
                    {
                        new()
                        {
                            Name = "Example Service",
                            Logo = "assets/tools/sample.png",
                            Subtitle = "This is a sample service",
                            Tag = "app",
                            Keywords = "self hosted reddit",
                            Url = "https://example.com",
                            Target = "_blank"
                        }
                    }
                }
            }
        };
    }

    public void Dispose()
    {
        _fileWatcher?.Dispose();
    }
}

public class ConfigurationOptions
{
    public string? ConfigPath { get; set; }
}
