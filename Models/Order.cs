using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBreadPit.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string UserName { get; set; } // Property to store the username

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Initialize OrderItems

        // Constructor to set the username when creating an order
        public Order(string userName)
        {
            UserName = userName; // Set the username from the parameter
        }
    }

    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        public int BroodjeId { get; set; } // Foreign key to the sandwich/broodje

        [ForeignKey("BroodjeId")] // Specify the foreign key relationship
        public Broodje Broodje { get; set; } // Navigation property to access the related Broodje

        public string BroodjeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        // Foreign key to the parent order
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
