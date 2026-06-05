using NUnit.Framework;
using ECommerceApp.Services;
using System;

namespace ECommerceApp.Tests
{
    [TestFixture]
    public class OrderBillingServiceTests
    {
        private OrderBillingService _orderBillingService;

        [SetUp]
        public void SetUp()
        {
            _orderBillingService = new OrderBillingService();
        }

       
        // CalculateSubTotal Tests
        

        [TestCase(1000, 5, 5000)]
        [TestCase(500, 2, 1000)]
        [TestCase(250, 4, 1000)]
        public void When_CalculateSubTotal_ValidInputs_ReturnsSubTotal( decimal productPrice,int quantity, decimal expectedSubTotal)
        {
            decimal result = _orderBillingService.CalculateSubTotal( productPrice,quantity);

            Assert.That(result, Is.EqualTo(expectedSubTotal));
        }

        [TestCase(0)]
        [TestCase(-100)]
        [TestCase(-500)]
        public void When_CalculateSubTotal_InvalidProductPrice_ThrowsArgumentException( decimal productPrice)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => _orderBillingService.CalculateSubTotal(productPrice, 5));

            Assert.That(exception.Message, Is.EqualTo("Product price must be greater than zero."));
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-10)]
        public void When_CalculateSubTotal_InvalidQuantity_ThrowsArgumentException(int quantity)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => _orderBillingService.CalculateSubTotal(1000,quantity));

            Assert.That(exception.Message,Is.EqualTo("Quantity must be greater than zero."));
        }

        
        // CalculateDiscount Tests
        

        [TestCase(5000, 500)]
        [TestCase(6000, 600)]
        [TestCase(10000, 1000)]
        public void When_CalculateDiscount_SubTotalGreaterThanOrEqual5000_Returns10PercentDiscount( decimal subTotal, decimal expectedDiscount)
        {
            decimal result = _orderBillingService.CalculateDiscount( subTotal);

            Assert.That(result, Is.EqualTo(expectedDiscount));
        }

        [TestCase(2000, 100)]
        [TestCase(3000, 150)]
        [TestCase(4500, 225)]
        public void When_CalculateDiscount_SubTotalBetween2000And4999_Returns5PercentDiscount( decimal subTotal, decimal expectedDiscount)
        {
            decimal result = _orderBillingService.CalculateDiscount( subTotal);

            Assert.That(result, Is.EqualTo(expectedDiscount));
        }

        [TestCase(1999)]
        [TestCase(1000)]
        [TestCase(500)]
        public void When_CalculateDiscount_SubTotalLessThan2000_ReturnsZero( decimal subTotal)
        {
            decimal result = _orderBillingService.CalculateDiscount(subTotal);

            Assert.That(result, Is.EqualTo(0));
        }

       
        // CalculateDeliveryCharge Tests
      

        [TestCase(500)]
        [TestCase(750)]
        [TestCase(999)]
        public void When_CalculateDeliveryCharge_AmountLessThan1000_Returns100( decimal amountAfterDiscount)
        {
            decimal result = _orderBillingService.CalculateDeliveryCharge(amountAfterDiscount);

            Assert.That(result, Is.EqualTo(100));
        }

        [TestCase(1000)]
        [TestCase(1500)]
        [TestCase(5000)]
        public void When_CalculateDeliveryCharge_AmountGreaterThanOrEqual1000_ReturnsZero(decimal amountAfterDiscount)
        {
            decimal result = _orderBillingService.CalculateDeliveryCharge(amountAfterDiscount);

            Assert.That(result, Is.EqualTo(0));
        }

        // CalculateFinalAmount Tests
       

        [TestCase(1000, 5, 4500)]
        [TestCase(500, 2, 1000)]
        [TestCase(300, 3, 1000)]
        [TestCase(2500, 1, 2375)]
        public void When_CalculateFinalAmount_ValidInputs_ReturnsFinalAmount( decimal productPrice, int quantity, decimal expectedFinalAmount)
        {
            decimal result = _orderBillingService.CalculateFinalAmount( productPrice,quantity);

            Assert.That(result, Is.EqualTo(expectedFinalAmount));
        }

        [Test]
        public void When_CalculateFinalAmount_InvalidProductPrice_ThrowsArgumentException()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => _orderBillingService.CalculateFinalAmount(0,5));

            Assert.That(exception.Message,Is.EqualTo("Product price must be greater than zero."));
        }

        [Test]
        public void When_CalculateFinalAmount_InvalidQuantity_ThrowsArgumentException()
        {
            ArgumentException exception =Assert.Throws<ArgumentException>(() =>  _orderBillingService.CalculateFinalAmount(1000,0));

            Assert.That(exception.Message, Is.EqualTo("Quantity must be greater than zero."));
        }
    }
}