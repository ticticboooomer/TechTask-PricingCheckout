using TechTask.Backend.Model;

namespace TechTask.Backend.Data;

public class DataAccessor : IDataAccessor
{
    private readonly Dictionary<string, ItemPricing> _prices = new();
    
    public void StorePricing(ItemPricing pricing)
    {
        _prices[pricing.Item] = pricing;
    }
}