using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectBreadPit.Models
{
    public class Broodje
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageName { get; set; }

        [Column(TypeName = "decimal(6, 2)")]
        public decimal Price { get; set; }
    }

}
