﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Northwind.Mvc.Models;
using Packt.Shared;
using Microsoft.EntityFrameworkCore;

namespace Northwind.Mvc.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly NorthwindContext db;
    public HomeController(ILogger<HomeController> logger, NorthwindContext db)
    {
        _logger = logger;        this.db = db;

    }

    public IActionResult Index(string? id = null, string? country = null)
    {
        IEnumerable<Order> model = db.Orders
            .Include(order => order.Customer)
            .Include(order => order.OrderDetails);

        if (id != null)
        {
            model = model.Where(order => order.Customer?.CustomerId == id);
        }
        if (country != null)
        {
            model = model.Where(order => order.Customer?.Country == country);
        }
        model = model
            .OrderByDescending(order => order.OrderDetails
            .Sum(detail => detail.Quantity * detail.UnitPrice))
            .AsEnumerable();

        return View(model);
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

    public IActionResult Shipper(Shipper shipper)
    {
        return View(shipper);
    }
}
