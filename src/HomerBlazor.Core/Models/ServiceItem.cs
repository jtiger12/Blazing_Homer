using YamlDotNet.Serialization;

namespace HomerBlazor.Core.Models;

public class ServiceItem
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; } = string.Empty;

    [YamlMember(Alias = "logo")]
    public string? Logo { get; set; }

    [YamlMember(Alias = "icon")]
    public string? Icon { get; set; }

    [YamlMember(Alias = "subtitle")]
    public string? Subtitle { get; set; }

    [YamlMember(Alias = "tag")]
    public string? Tag { get; set; }

    [YamlMember(Alias = "tagstyle")]
    public string? TagStyle { get; set; }

    [YamlMember(Alias = "keywords")]
    public string? Keywords { get; set; }

    [YamlMember(Alias = "url")]
    public string? Url { get; set; }

    [YamlMember(Alias = "target")]
    public string Target { get; set; } = "_self";

    [YamlMember(Alias = "type")]
    public string? Type { get; set; }

    [YamlMember(Alias = "endpoint")]
    public string? Endpoint { get; set; }

    [YamlMember(Alias = "useCredentials")]
    public bool UseCredentials { get; set; }

    [YamlMember(Alias = "headers")]
    public Dictionary<string, string>? Headers { get; set; }

    [YamlMember(Alias = "class")]
    public string? Class { get; set; }

    [YamlMember(Alias = "background")]
    public string? Background { get; set; }

    // For additional service-specific properties
    [YamlIgnore]
    public Dictionary<string, object> AdditionalProperties { get; set; } = new();

    // Service card specific properties
    [YamlMember(Alias = "apikey")]
    public string? ApiKey { get; set; }

    [YamlMember(Alias = "username")]
    public string? Username { get; set; }

    [YamlMember(Alias = "password")]
    public string? Password { get; set; }

    [YamlMember(Alias = "token")]
    public string? Token { get; set; }

    [YamlMember(Alias = "refresh")]
    public int? Refresh { get; set; }

    [YamlMember(Alias = "timeout")]
    public int? Timeout { get; set; }
}
