using YamlDotNet.Serialization;

namespace HomerBlazor.Core.Models;

public class DashboardConfig
{
    [YamlMember(Alias = "title")]
    public string Title { get; set; } = "Homer Dashboard";

    [YamlMember(Alias = "subtitle")]
    public string? Subtitle { get; set; }

    [YamlMember(Alias = "documentTitle")]
    public string? DocumentTitle { get; set; }

    [YamlMember(Alias = "logo")]
    public string? Logo { get; set; }

    [YamlMember(Alias = "icon")]
    public string? Icon { get; set; }

    [YamlMember(Alias = "header")]
    public bool Header { get; set; } = true;

    [YamlMember(Alias = "footer")]
    public string? Footer { get; set; }

    [YamlMember(Alias = "columns")]
    public string Columns { get; set; } = "auto";

    [YamlMember(Alias = "connectivityCheck")]
    public bool ConnectivityCheck { get; set; } = true;

    [YamlMember(Alias = "proxy")]
    public ProxyConfig? Proxy { get; set; }

    [YamlMember(Alias = "defaults")]
    public DefaultsConfig? Defaults { get; set; }

    [YamlMember(Alias = "theme")]
    public string Theme { get; set; } = "default";

    [YamlMember(Alias = "stylesheet")]
    public List<string>? Stylesheet { get; set; }

    [YamlMember(Alias = "colors")]
    public ColorsConfig? Colors { get; set; }

    [YamlMember(Alias = "message")]
    public MessageConfig? Message { get; set; }

    [YamlMember(Alias = "links")]
    public List<LinkConfig>? Links { get; set; }

    [YamlMember(Alias = "services")]
    public List<ServiceGroup> Services { get; set; } = new();
}

public class ProxyConfig
{
    [YamlMember(Alias = "useCredentials")]
    public bool UseCredentials { get; set; }
}

public class DefaultsConfig
{
    [YamlMember(Alias = "layout")]
    public string? Layout { get; set; }

    [YamlMember(Alias = "colorTheme")]
    public string? ColorTheme { get; set; }
}

public class ColorsConfig
{
    [YamlMember(Alias = "light")]
    public Dictionary<string, string>? Light { get; set; }

    [YamlMember(Alias = "dark")]
    public Dictionary<string, string>? Dark { get; set; }
}

public class MessageConfig
{
    [YamlMember(Alias = "url")]
    public string? Url { get; set; }

    [YamlMember(Alias = "mapping")]
    public Dictionary<string, string>? Mapping { get; set; }

    [YamlMember(Alias = "refresh")]
    public int Refresh { get; set; } = 10000;

    [YamlMember(Alias = "style")]
    public string? Style { get; set; }

    [YamlMember(Alias = "title")]
    public string? Title { get; set; }

    [YamlMember(Alias = "icon")]
    public string? Icon { get; set; }

    [YamlMember(Alias = "content")]
    public string? Content { get; set; }
}

public class LinkConfig
{
    [YamlMember(Alias = "name")]
    public string Name { get; set; } = string.Empty;

    [YamlMember(Alias = "icon")]
    public string? Icon { get; set; }

    [YamlMember(Alias = "url")]
    public string Url { get; set; } = string.Empty;

    [YamlMember(Alias = "target")]
    public string Target { get; set; } = "_blank";
}
