using Hangfire;
using Hangfire.SqlServer;
using ProjectBreadPit.Data;

namespace ProjectBreadPit.Background
{
    public class Purge
    {
        private readonly BreadPitContext _context;

        [AutomaticRetry(Attempts = 0)]
        public void DeleteOrdersAndOrderItems()
        {
            var orders = _context.Orders.ToList();
            _context.Orders.RemoveRange(orders);
            _context.SaveChanges();
        }
    }

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // Add Hangfire services
            services.AddHangfire(config => config.UseSqlServerStorage("Server=(localdb)\\mssqllocaldb;Database=ProjectBreadPit;Trusted_Connection=True;"));
        }

        public void Configure(IApplicationBuilder app)
        {
            // Use Hangfire dashboard (optional)
            app.UseHangfireDashboard();

            // Schedule the job to run every day at 2:30 PM
            RecurringJob.AddOrUpdate<Purge>("OrderCleanupJob",
                                                         x => x.DeleteOrdersAndOrderItems(),
                                                         "30 14 * * *");
        }
    }
}
