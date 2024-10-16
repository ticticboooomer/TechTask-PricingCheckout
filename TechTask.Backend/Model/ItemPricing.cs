namespace TechTask.Backend.Model;

public record ItemPricing(
    string Item,
    int UnitPrice,
    List<SpecialPrice> SpecialPrices);