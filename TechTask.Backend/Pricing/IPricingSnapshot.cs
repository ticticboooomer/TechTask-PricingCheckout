using TechTask.Backend.Model;

namespace TechTask.Backend.Pricing;

public interface IPricingSnapshot
{
    ItemPricing GetItemPricing(string item);
}