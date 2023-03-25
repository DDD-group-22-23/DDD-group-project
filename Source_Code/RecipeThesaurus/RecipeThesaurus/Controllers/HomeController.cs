using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using RecipeThesaurus.Models;

namespace RecipeThesaurus.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Recommend()
    {
        return View();
    }

    public IActionResult Settings()
    {
        return View();
    }

    public IActionResult Saved()
    {
        return View();
    }

    public IActionResult People()
    {
        return View();
    }

    public IActionResult Fridge()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

