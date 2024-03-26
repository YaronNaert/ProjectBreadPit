using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System.Collections.Generic;
using System.Linq;


namespace ProjectBreadPit.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // Retrieve cart from session or create a new one if it doesn't exist
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int broodjeId)
        {
            // Retrieve cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            // Remove the item from the cart based on broodjeId
            var itemToRemove = cart.FirstOrDefault(item => item.BroodjeId == broodjeId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);

                // Save the updated cart back to session
                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Index");
        }
    }
}