namespace TechTask.Backend.Checkout;

public class Basket : IBasket
{
    private readonly Dictionary<string, int> _scannedItems = new();
    
    public void ScanItem(string item)
    {
        _scannedItems[item] = _scannedItems.GetValueOrDefault(item, 0) + 1;
    }

    public int GetTotalPrice()
    {
        return 0;
    }
}