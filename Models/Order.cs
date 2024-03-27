namespace ProjectBreadPit.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public string CustomerName { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderItem> OrderItems { get; set; } // Represents the items ordered in the order

        // Other properties as needed, such as total amount, shipping address, etc.
    }

    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int BroodjeId { get; set; } // Foreign key to the sandwich/broodje
        public string BroodjeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Navigation property to the parent order
        public Order Order { get; set; }
    }
}
