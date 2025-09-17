using HomerBlazor.Core.Models;
using HomerBlazor.ServiceCards.Interfaces;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.Extensions.Logging;

namespace HomerBlazor.ServiceCards.Base;

public abstract class BaseServiceCard : ComponentBase, IServiceCard
{
    [Parameter] public ServiceItem Config { get; set; } = new();
    [Parameter] public ServiceCardData Data { get; set; } = new();
    [Inject] protected IHttpClientFactory HttpClientFactory { get; set; } = default!;
    [Inject] protected ILogger<BaseServiceCard> Logger { get; set; } = default!;
    
    public abstract string CardType { get; }
    public virtual TimeSpan DefaultRefreshInterval => TimeSpan.FromMinutes(5);
    
    public abstract Task<ServiceCardData> GetCardDataAsync(ServiceItem config, CancellationToken cancellationToken);
    
    public virtual RenderFragment RenderCard(ServiceItem config, ServiceCardData data) => builder =>
    {
        builder.OpenElement(0, "div");
        builder.AddAttribute(1, "class", "smart-card");
        builder.AddContent(2, RenderCardContent(config, data));
        builder.CloseElement();
    };

    protected abstract RenderFragment RenderCardContent(ServiceItem config, ServiceCardData data);
    
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.AddContent(0, RenderCard(Config, Data));
    }

    protected virtual async Task<T> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default)
    {
        using var client = HttpClientFactory.CreateClient();
        
        // Configure client with headers if needed
        if (Config.Headers != null)
        {
            foreach (var header in Config.Headers)
            {
                client.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }

        // Set timeout
        client.Timeout = TimeSpan.FromSeconds(30);

        try
        {
            var response = await client.GetAsync(endpoint, cancellationToken);
            response.EnsureSuccessStatusCode();
            
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            return System.Text.Json.JsonSerializer.Deserialize<T>(content, new System.Text.Json.JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            }) ?? throw new InvalidOperationException("Failed to deserialize response");
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to fetch data from {Endpoint}", endpoint);
            throw;
        }
    }
}