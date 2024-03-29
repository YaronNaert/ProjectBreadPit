using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System.Collections.Generic;
using System.Linq;

//[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly BreadPitContext _context;


    public AdminController(BreadPitContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var broodje = _context.broodjes.ToList();
        return View(broodje);
    }

    [HttpGet]
    public IActionResult New()
    {
        return View();
    }

    [HttpPost]
    public IActionResult New(Broodje broodje)
    {
        if (ModelState.IsValid)
        {
            _context.broodjes.Add(broodje);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
        return View(broodje);
    }
    public IActionResult OrderManager()
    {
        var orders = _context.Orders.Include(o => o.OrderItems).ToList();
        return View(orders);
    }

    public IActionResult Edit(int id)
    {
        var order = _context.Orders.Include(o => o.OrderItems).SingleOrDefault(o => o.OrderId == id);
        if (order == null)
        {
            return NotFound();
        }

        // Retrieve list of available sandwiches
        var broodjes = _context.broodjes.ToList();
        ViewBag.Broodjes = broodjes;

        return View(order);
    }


    [HttpPost]
    public IActionResult Edit(Order order)
    {
        if (ModelState.IsValid)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
            return RedirectToAction("OrderManager");
        }
        return View(order);
    }

    [HttpPost]
    public IActionResult Delete(int id)
    {
        var order = _context.Orders.Find(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        _context.SaveChanges();

        return Ok(); 
    }

    [HttpPost]
    public IActionResult RemoveItem(int orderId, int orderItemId)
    {
        var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            return NotFound();
        }

        var orderItem = order.OrderItems.FirstOrDefault(oi => oi.OrderItemId == orderItemId);
        if (orderItem == null)
        {
            return NotFound();
        }

        _context.OrderItems.Remove(orderItem);
        _context.SaveChanges();

        return Json(new { success = true });
    }


    [HttpPost]
    public IActionResult AddItem(int orderId, int broodjeId, int quantity)
    {
        var order = _context.Orders.Include(o => o.OrderItems).FirstOrDefault(o => o.OrderId == orderId);
        if (order == null)
        {
            return NotFound();
        }

        var existingOrderItem = order.OrderItems.FirstOrDefault(oi => oi.BroodjeId == broodjeId);
        if (existingOrderItem != null)
        {
            // If the order item already exists, update its quantity
            existingOrderItem.Quantity += quantity;
        }
        else
        {
            // If the order item does not exist, create a new one
            var broodje = _context.broodjes.FirstOrDefault(b => b.Id == broodjeId);
            if (broodje == null)
            {
                return NotFound();
            }

            var orderItem = new OrderItem
            {
                BroodjeId = broodjeId,
                BroodjeName = broodje.Name,
                Price = broodje.Price,
                Quantity = quantity
            };

            order.OrderItems.Add(orderItem);
        }

        _context.SaveChanges();

        // You can return any necessary data or status here
        return Json(new { success = true });
    }



}