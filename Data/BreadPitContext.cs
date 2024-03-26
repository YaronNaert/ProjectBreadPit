using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Data
{
    public class BreadPitContext : DbContext
    {
        public DbSet<Sandwich> broodjes { get; set;} = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Initial Catalog=ProjectBreadPit;Integrated Security=True;");
        }

    }
}
