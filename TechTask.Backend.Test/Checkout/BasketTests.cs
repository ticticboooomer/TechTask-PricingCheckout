using FluentAssertions;
using Moq;
using TechTask.Backend.Checkout;
using TechTask.Backend.Model;
using TechTask.Backend.Pricing;

namespace TechTask.Backend.Test.Checkout;

public class BasketTests
{
    private readonly Mock<IPricingSnapshot> _pricingSnapshot;

    public BasketTests()
    {
        _pricingSnapshot = new Mock<IPricingSnapshot>();
        _pricingSnapshot.Setup(x => x.GetItemPricing(It.Is<string>(v => v == "A")))
            .Returns(new ItemPricing("A", 50, new() { { 3, 130 } }));
        _pricingSnapshot.Setup(x => x.GetItemPricing(It.Is<string>(v => v == "B")))
            .Returns(new ItemPricing("B", 30, new() { { 2, 45 } }));
        _pricingSnapshot.Setup(x => x.GetItemPricing(It.Is<string>(v => v == "C")))
            .Returns(new ItemPricing("C", 20, new ()));
        _pricingSnapshot.Setup(x => x.GetItemPricing(It.Is<string>(v => v == "D")))
            .Returns(new ItemPricing("D", 15, new ()));
    }

    [Theory]
    [InlineData(new[] { "A", "B", "B", "D" }, 110)]
    [InlineData(new[] { "A", "A", "A", "B", "B" }, 175)]
    [InlineData(new[] { "A", "A", "A", "B", "C", "C" }, 200)]
    public void GetTotalPrice_ScanningAvailableItems_ReturnCorrectSum(string[] items, int total)
    {
        var basket = new Basket(_pricingSnapshot.Object);
        foreach (var item in items)
        {
            basket.ScanItem(item);
        }

        var totalPrice = basket.GetTotalPrice();

        totalPrice.Should().Be(total);
        totalPrice.Should().BePositive();
    }
}