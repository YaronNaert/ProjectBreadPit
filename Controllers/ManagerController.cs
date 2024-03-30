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

    public IActionResult Index()
    {
        var orders = _context.Orders.Include(o => o.OrderItems).ToList();
        return View(orders);
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

    public IActionResult TotalSandwiches()
    {
        // Query the database to get the total quantity of each sandwich ordered
        var sandwiches = _context.broodjes.Include(b => b.OrderItems).ToList();

        return View(sandwiches);
    }

}