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
        _pricingSnapshot.Setup(x => x.ItemExists(It.IsAny<string>()))
            .Returns(false);
        _pricingSnapshot.Setup(x => x.ItemExists(It.IsIn("A", "B", "C", "D")))
            .Returns(true);
    }

    [Theory]
    [InlineData(new[] { "A", "B", "B", "D" }, 110)]
    [InlineData(new[] { "A", "A", "A", "B", "B" }, 175)]
    [InlineData(new[] { "A", "A", "A", "B", "C", "C" }, 200)]
    [InlineData(new[] { "C", "C", "B", "D", "A", "B" }, 150)]
    [InlineData(new[] { "B", "A", "C", "B", "A", "B", "A" }, 240)]
    [InlineData(new[] { "D", "B", "C", "A", "B" }, 130)]
    public void GetTotalPrice_ScanningAvailableItems_ReturnCorrectSum(string[] items, int total)
    {
        var basket = new Basket(_pricingSnapshot.Object);
        foreach (var item in items)
        {
            basket.ScanItem(item);
        }

        var totalPrice = basket.GetTotalPrice();

        totalPrice.Should().Be(total, "Is the correct value")
            .And.BePositive("Prices can never be negative");
    }

    [Theory]
    [InlineData("7")]
    [InlineData("R")]
    [InlineData(";")]
    [InlineData("F")]
    public void ScanItem_InvalidItem_ThrowException(string item)
    {
        var basket = new Basket(_pricingSnapshot.Object);
        
        var act = () => basket.ScanItem(item);

        act.Should().Throw<InvalidOperationException>("Items without prices are not allowed");
    }

    [Theory]
    [InlineData("A")]
    [InlineData("B")]
    [InlineData("C")]
    [InlineData("D")]
    public void ScanItem_ValidItem_RunCorrectly(string item)
    {
        var basket = new Basket(_pricingSnapshot.Object);

        var act = () => basket.ScanItem(item);

        act.Should().NotThrow("Valid Items should be accepted quielty");
    }
}