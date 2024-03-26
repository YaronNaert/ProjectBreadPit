using Microsoft.AspNetCore.Mvc;
using ProjectBreadPit.Models;

namespace ProjectBreadPit.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(User user)
        {
            if (user.Username == "Yaron" && user.Password == "Password")
            {
                // Authentication successful, redirect to dashboard
                return RedirectToAction("Dashboard", "Dashboard");
            }
            else
            {
                // Authentication failed, return to login page with error message
                ViewBag.ErrorMessage = "Invalid username or password.";
                return View();
            }
        }

        public ActionResult Dashboard()
        {
            // This is the dashboard page
            return View("Dashboard");
        }
    }
}
