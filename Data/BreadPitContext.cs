using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Data
{
    public class BreadPitContext : DbContext
    {

        public BreadPitContext(DbContextOptions<BreadPitContext> options) : base(options)
        {
        }

        public DbSet<Broodje> broodjes { get; set;} = null!;
        public DbSet<Order> orders { get; set; } = null!;



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectBreadPit;Integrated Security=True;");
        }

    }
}
