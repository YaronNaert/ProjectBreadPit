using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Data
{
    public class BreadPitContext : DbContext
    {
        public BreadPitContext(DbContextOptions<BreadPitContext> options) : base(options)
        {
        }

        public DbSet<Broodje>? broodjes { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; } // Changed DbSet name to OrderItems
        public DbSet<Order> Orders { get; set; } // Added DbSet for Orders

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the Price property of OrderItem entity to use SQL Server decimal column type
            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18, 2)");

            // Define the one-to-many relationship between Broodje and OrderItem
            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Broodje)
                .WithMany(b => b.OrderItems)
                .HasForeignKey(o => o.BroodjeId);
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectBreadPit;Integrated Security=True;");
        }
    }
}
