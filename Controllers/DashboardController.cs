using Microsoft.AspNetCore.Mvc;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            List<Sandwich> sandwiches = new List<Sandwich>
            {
                new Sandwich { Name = "Préparé", ImageUrl = "/images/Préparé.png", Price = "4 euro" },
                new Sandwich { Name = "Turkey Club", ImageUrl = "turkey_club.jpg", Price = "5 euro" },
            };
            return View(sandwiches);
        }
    }
}
