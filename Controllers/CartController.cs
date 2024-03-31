using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;


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
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            return View(cart);
        }

        [HttpPost]
        public IActionResult RemoveFromCart(int broodjeId)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var itemToRemove = cart.FirstOrDefault(item => item.BroodjeId == broodjeId);
            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateQuantity(int broodjeId, int quantity)
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var itemToUpdate = cart.FirstOrDefault(item => item.BroodjeId == broodjeId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;

                HttpContext.Session.SetString("Cart", JsonConvert.SerializeObject(cart));
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetCartDetails()
        {
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            decimal totalPrice = cart.Sum(item => item.Price * item.Quantity);

            var cartDetails = new
            {
                ItemsHtml = RenderCartItemsHtml(cart),
                Total = totalPrice.ToString("C")
            };

            return Json(cartDetails);
        }

        private string RenderCartItemsHtml(List<CartItem> cart)
        {
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

            if (!User.Identity.IsAuthenticated)
            {
                ViewData["ErrorMessage"] = "You must be logged in to place an order.";
                return View("WarningView"); 
            }
            var cartJson = HttpContext.Session.GetString("Cart");
            var cart = string.IsNullOrEmpty(cartJson) ? new List<CartItem>() : JsonConvert.DeserializeObject<List<CartItem>>(cartJson);

            var userName = User.Identity.Name;
            var order = new Order(userName);

            foreach (var cartItem in cart)
            {
                var orderItem = new OrderItem
                {
                    BroodjeId = cartItem.BroodjeId,
                    BroodjeName = cartItem.BroodjeName,
                    Price = cartItem.Price,
                    Quantity = cartItem.Quantity
                };

                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(order);
            _context.SaveChanges();

            HttpContext.Session.Remove("Cart");

            return RedirectToAction("Index", "Home");
        }



    }
}