using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System;
using System.Diagnostics;

namespace ProjectBreadPit.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BreadPitContext _context;

        public HomeController(ILogger<HomeController> logger, BreadPitContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _context.broodjes.ToListAsync();
            return View(products);
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult AddToCart(int broodjeId, string broodjeName, decimal price, int quantity)
        {
            // Retrieve cart from session or create a new one if it doesn't exist
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            // Check if the item is already in the cart
            var existingItem = cart.FirstOrDefault(item => item.BroodjeId == broodjeId);
            if (existingItem != null)
            {
                // Update quantity if item is already in the cart
                existingItem.Quantity += quantity;
            }
            else
            {
                // Add new item to the cart
                cart.Add(new CartItem
                {
                    BroodjeId = broodjeId,
                    BroodjeName = broodjeName,
                    Price = price,
                    Quantity = quantity
                });
            }

            // Save the updated cart back to session
            HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));

            // Redirect to the home page
            return RedirectToAction("Index");
        }

    }
}