using ECommerceApp.Models;

namespace ECommerceApp.Services
{
    public class OrderBillingService
    {
        public decimal CalculateSubTotal(decimal productPrice, int quantity)
        {
            if (productPrice <= 0)
            {
                throw new ArgumentException("Product price must be greater than zero.");
            }

            if (quantity <= 0)
            {
                throw new ArgumentException("Quantity must be greater than zero.");
            }

            return productPrice * quantity;
        }

        public decimal CalculateDiscount(decimal subTotal)
        {
            if (subTotal >= 5000)
            {
                return subTotal * 0.10m;
            }

            if (subTotal >= 2000)
            {
                return subTotal * 0.05m;
            }

            return 0;
        }

        public decimal CalculateDeliveryCharge(decimal amountAfterDiscount)
        {
            if (amountAfterDiscount < 1000)
            {
                return 100;
            }

            return 0;
        }

        public decimal CalculateFinalAmount(decimal productPrice, int quantity)
        {
            decimal subTotal =
                CalculateSubTotal(productPrice, quantity);

            decimal discount =
                CalculateDiscount(subTotal);

            decimal amountAfterDiscount =
                subTotal - discount;

            decimal deliveryCharge =
                CalculateDeliveryCharge(amountAfterDiscount);

            return amountAfterDiscount + deliveryCharge;
        }
    }
}