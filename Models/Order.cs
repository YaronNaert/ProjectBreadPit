using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBreadPit.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderId { get; set; }

        public string UserName { get; set; }

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order(string userName)
        {
            UserName = userName;
        }
    }

    public class OrderItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderItemId { get; set; }

        public int BroodjeId { get; set; }

        [ForeignKey("BroodjeId")]
        public Broodje Broodje { get; set; } 

        public string BroodjeName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}
