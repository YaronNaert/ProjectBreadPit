using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    // Add other action methods for updating and deleting products
}