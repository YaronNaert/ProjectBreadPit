namespace ProjectBreadPit.Models
{
    public class CartItem
    {
        public int BroodjeId { get; set; }
        public string BroodjeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
    public class ShoppingCart
    {
        public List<CartItem> Items { get; set; }
    }
}
