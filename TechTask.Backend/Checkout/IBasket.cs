namespace TechTask.Backend.Checkout;

public interface IBasket
{
    void ScanItem(string item);
    int GetTotalPrice();
}