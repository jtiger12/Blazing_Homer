using FluentAssertions;
using HomerBlazor.Core.Models;
using HomerBlazor.Core.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace HomerBlazor.Tests;

public class ConfigurationServiceTests
{
    private readonly Mock<ILogger<ConfigurationService>> _mockLogger;
    private readonly ConfigurationService _configurationService;
    private readonly string _testConfigPath;

    public ConfigurationServiceTests()
    {
        _mockLogger = new Mock<ILogger<ConfigurationService>>();
        _testConfigPath = Path.Combine(Path.GetTempPath(), "test-config.yml");
        
        var options = Options.Create(new ConfigurationOptions
        {
            ConfigPath = _testConfigPath
        });
        
        _configurationService = new ConfigurationService(_mockLogger.Object, options);
    }

    [Fact]
    public async Task LoadConfigurationAsync_WhenFileDoesNotExist_CreatesDefaultConfiguration()
    {
        // Arrange
        if (File.Exists(_testConfigPath))
        {
            File.Delete(_testConfigPath);
        }

        // Act
        var config = await _configurationService.LoadConfigurationAsync();

        // Assert
        config.Should().NotBeNull();
        config.Title.Should().Be("Homer Dashboard");
        config.Services.Should().NotBeEmpty();
        File.Exists(_testConfigPath).Should().BeTrue();
    }

    [Fact]
    public async Task ValidateConfigurationAsync_WithValidConfig_ReturnsTrue()
    {
        // Arrange
        var config = new DashboardConfig
        {
            Title = "Test Dashboard",
            Services = new List<ServiceGroup>
            {
                new()
                {
                    Name = "Test Group",
                    Items = new List<ServiceItem>
                    {
                        new() { Name = "Test Service" }
                    }
                }
            }
        };

        // Act
        var result = await _configurationService.ValidateConfigurationAsync(config);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task ValidateConfigurationAsync_WithEmptyTitle_ReturnsFalse()
    {
        // Arrange
        var config = new DashboardConfig
        {
            Title = "",
            Services = new List<ServiceGroup>()
        };

        // Act
        var result = await _configurationService.ValidateConfigurationAsync(config);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        if (File.Exists(_testConfigPath))
        {
            File.Delete(_testConfigPath);
        }
    }
}
