using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;



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
        var sandwichQuantities = _context.OrderItems
            .GroupBy(oi => oi.BroodjeId)
            .Select(g => new { BroodjeId = g.Key, TotalQuantity = g.Sum(oi => oi.Quantity) })
            .ToList();

        var sandwiches = _context.broodjes
            .Where(b => sandwichQuantities.Any(sq => sq.BroodjeId == b.Id))
            .Select(b => new BroodjeWithTotalQuantity
            {
                Broodje = b,
                TotalQuantity = sandwichQuantities.FirstOrDefault(sq => sq.BroodjeId == b.Id) != null ? sandwichQuantities.FirstOrDefault(sq => sq.BroodjeId == b.Id).TotalQuantity : 0
            })
            .ToList();

        return View(sandwiches);
    }

}