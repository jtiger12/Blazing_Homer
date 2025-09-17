using HomerBlazor.ServiceCards.Interfaces;

namespace HomerBlazor.ServiceCards.Services;

public interface IServiceCardRegistry
{
    void RegisterCard<T>(string cardType) where T : class, IServiceCard;
    Type? GetCardType(string cardType);
    IEnumerable<string> GetAvailableCardTypes();
    IServiceCard? CreateCard(string cardType);
}

public class ServiceCardRegistry : IServiceCardRegistry
{
    private readonly Dictionary<string, Type> _cardTypes = new();
    private readonly IServiceProvider _serviceProvider;

    public ServiceCardRegistry(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public void RegisterCard<T>(string cardType) where T : class, IServiceCard
    {
        _cardTypes[cardType.ToLowerInvariant()] = typeof(T);
    }

    public Type? GetCardType(string cardType)
    {
        return _cardTypes.TryGetValue(cardType.ToLowerInvariant(), out var type) ? type : null;
    }

    public IEnumerable<string> GetAvailableCardTypes()
    {
        return _cardTypes.Keys;
    }

    public IServiceCard? CreateCard(string cardType)
    {
        var type = GetCardType(cardType);
        if (type == null) return null;

        return (IServiceCard?)_serviceProvider.GetService(type);
    }
}