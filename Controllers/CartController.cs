using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System.Collections.Generic;
using System.Linq;


namespace ProjectBreadPit.Controllers
{
    public class CartController : Controller
    {
        private readonly BreadPitContext _context;

        public CartController(BreadPitContext context)
        {
            _context = context;
        }

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
        [HttpGet]
        public IActionResult GetCartDetails()
        {
            // Retrieve cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            // Calculate total price
            decimal totalPrice = cart.Sum(item => item.Price * item.Quantity);

            // Prepare data to send to the client
            var cartDetails = new
            {
                ItemsHtml = RenderCartItemsHtml(cart),
                Total = totalPrice.ToString("C") // Format total price as currency
            };

            return Json(cartDetails);
        }

        private string RenderCartItemsHtml(List<CartItem> cart)
        {
            // Create HTML string for cart items
            string itemsHtml = "";
            foreach (var item in cart)
            {
                itemsHtml += $"<li>{item.BroodjeName} - Quantity: {item.Quantity} - Price: {item.Price * item.Quantity:C}</li>";
            }
            return itemsHtml;
        }

        [HttpPost]
        public IActionResult PlaceOrder()
        {
            // Check if the user is authenticated
            if (!User.Identity.IsAuthenticated)
            {
                // If the user is not authenticated, display a warning message
                ViewData["ErrorMessage"] = "You must be logged in to place an order.";
                return View("WarningView"); // Return a view with the warning message
            }

            // Retrieve cart from session
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            // Create a new order
            var userName = User.Identity.Name;
            var order = new Order(userName);

            // Save the items from the cart to the database
            foreach (var cartItem in cart)
            {
                // Create a new OrderItem object and map properties from the cartItem
                var orderItem = new OrderItem
                {
                    BroodjeId = cartItem.BroodjeId,
                    BroodjeName = cartItem.BroodjeName,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity
                };

                // Add the order item to the order's collection
                order.OrderItems.Add(orderItem);
            }

            // Add the order to the context and save changes
            _context.Orders.Add(order);
            _context.SaveChanges();

            // Clear the cart after placing the order
            HttpContext.Session.Remove("Cart");

            // Redirect to the desired action
            return RedirectToAction("Index", "Home");
        }


    }
}