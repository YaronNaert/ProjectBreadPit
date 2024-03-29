using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System.Collections.Generic;
using System.Linq;


public class ManagerController : Controller
{

    private readonly BreadPitContext _context;

    public ManagerController(BreadPitContext context)
    {
        _context = context;
    }

    public IActionResult OrdersOverview()
    {
        var orders = _context.Orders.ToList();
        return View(orders);
    }

    public IActionResult OrderDetails(int orderId)
    {
        var broodje = _context.Orders.ToList();
        return View(broodje);
    }

    // Add action method for viewing total sandwich orders
    public IActionResult TotalSandwichOrders()
    {
        // Logic to retrieve total sandwich orders
        // and pass them to the view
        return View();
    }
}