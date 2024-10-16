namespace TechTask.Backend.Model;

public record ItemPricing(
    string Item,
    int UnitPrice,
    Dictionary<int, int> SpecialPrices);