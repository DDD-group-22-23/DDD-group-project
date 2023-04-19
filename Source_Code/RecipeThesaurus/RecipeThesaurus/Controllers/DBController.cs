using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecipeThesaurus.Models;

namespace RecipeThesaurus.Controllers;

public class DBController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public DBController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

