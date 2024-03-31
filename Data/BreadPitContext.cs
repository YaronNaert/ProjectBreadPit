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
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderItem>()
                .Property(o => o.Price)
                .HasColumnType("decimal(18, 2)");

            modelBuilder.Entity<OrderItem>()
                .HasOne(o => o.Broodje)
                .WithMany() 
                .HasForeignKey(o => o.BroodjeId);
        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectBreadPit;Integrated Security=True;");
        }
    }
}
