using TechTask.Backend.Model;
using TechTask.Backend.Pricing;

namespace TechTask.Backend.Checkout;

public class Basket : IBasket
{
    private readonly IPricingSnapshot _snapshot;

    public Basket(IPricingSnapshot snapshot)
    {
        _snapshot = snapshot;
    }
    
    private readonly Dictionary<string, int> _scannedItems = new();
    
    public void ScanItem(string item)
    {
        _scannedItems[item] = _scannedItems.GetValueOrDefault(item, 0) + 1;
    }

    public int GetTotalPrice()
    {
        var total = 0;
        foreach (var scannedItem in _scannedItems)
        {
            var item = scannedItem.Key;
            var quantity = scannedItem.Value;

            var pricing = _snapshot.GetItemPricing(item);
            if (pricing.SpecialPrices.TryGetValue(quantity, out var specialPrice))
            {
                total += specialPrice;
            }
            else
            {
                total += pricing.UnitPrice * quantity;
            }
        }
        return total;
    }
}