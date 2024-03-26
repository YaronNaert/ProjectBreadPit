using Microsoft.AspNetCore.Mvc;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult Dashboard()
        {
            List<Broodje> sandwiches = new List<Broodje>
            {
                new Broodje { Name = "Préparé", ImageName = "Préparé.png", Price = 4.00M },
                new Broodje { Name = "Turkey Club", ImageName = "turkey_club.jpg", Price = 5.50M },
            };
            return View(sandwiches);
        }
    }
}
