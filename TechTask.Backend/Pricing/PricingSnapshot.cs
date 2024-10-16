using TechTask.Backend.Model;

namespace TechTask.Backend.Pricing;

public class PricingSnapshot : IPricingSnapshot
{
    private readonly Dictionary<string, ItemPricing> _prices;

    public PricingSnapshot(Dictionary<string, ItemPricing> prices)
    {
        _prices = prices;
    }

    public ItemPricing? GetItemPricing(string item)
    {
        return _prices.GetValueOrDefault(item, null);
    }

    public bool ItemExists(string item)
    {
        return _prices.ContainsKey(item);
    }
}