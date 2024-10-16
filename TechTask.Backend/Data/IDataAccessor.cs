using TechTask.Backend.Model;

namespace TechTask.Backend.Data;

public interface IDataAccessor
{
    void StorePricing(ItemPricing pricing);
}