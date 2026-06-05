namespace ECommerceApp.Models
{
    public class Order
    {
        public decimal ProductPrice { get; set; }
        public int Quantity { get; set; }

        public Order(decimal productPrice, int quantity)
        {
            ProductPrice = productPrice;
            Quantity = quantity;
        }
    }
}