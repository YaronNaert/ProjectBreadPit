﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectBreadPit.Data;
using ProjectBreadPit.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly BreadPitContext _context;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;


    public AdminController(BreadPitContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
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

            existingOrderItem.Quantity += quantity;
        }
        else
        {

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


        return Json(new { success = true });
    }
    public async Task<IActionResult> ManageRoles()
    {
        var users = await _userManager.Users.ToListAsync();
        var roles = await _roleManager.Roles.ToListAsync();

        // Convert roles to SelectListItem
        ViewBag.Roles = roles.Select(r => new SelectListItem { Text = r.Name, Value = r.Name }).ToList();

        ViewBag.UserManager = _userManager; // Pass UserManager to the view
        return View(users);
    }



    [HttpPost]
    public async Task<IActionResult> ChangeUserRole(string userId, string role)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        // Remove all existing roles for the user
        var roles = await _userManager.GetRolesAsync(user);
        await _userManager.RemoveFromRolesAsync(user, roles);

        // Add the selected role
        await _userManager.AddToRoleAsync(user, role);

        return RedirectToAction("ManageRoles");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteUser(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var result = await _userManager.DeleteAsync(user);
        if (result.Succeeded)
        {
            return RedirectToAction("ManageRoles");
        }
        else
        {
            // Handle errors if user deletion fails
            ModelState.AddModelError("", "Failed to delete user.");
            return RedirectToAction("ManageRoles");
        }
    }
}