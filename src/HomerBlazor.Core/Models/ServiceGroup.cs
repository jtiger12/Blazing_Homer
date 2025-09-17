using YamlDotNet.Serialization;

namespace HomerBlazor.Core.Models;

public class ServiceGroup
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; } = string.Empty;

    [YamlMember(Alias = "icon")]
    public string? Icon { get; set; }

    [YamlMember(Alias = "logo")]
    public string? Logo { get; set; }

    [YamlMember(Alias = "class")]
    public string? Class { get; set; }

    [YamlMember(Alias = "items")]
    public List<ServiceItem> Items { get; set; } = new();
}
