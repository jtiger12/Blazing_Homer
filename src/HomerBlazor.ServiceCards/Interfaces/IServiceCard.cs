using HomerBlazor.Core.Models;
using Microsoft.AspNetCore.Components;

namespace HomerBlazor.ServiceCards.Interfaces;

public interface IServiceCard
{
    string CardType { get; }
    Task<ServiceCardData> GetCardDataAsync(ServiceItem config, CancellationToken cancellationToken);
    RenderFragment RenderCard(ServiceItem config, ServiceCardData data);
    TimeSpan DefaultRefreshInterval { get; }
}