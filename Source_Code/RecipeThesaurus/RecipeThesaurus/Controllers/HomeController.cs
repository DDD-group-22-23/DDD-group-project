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
        // Connection needs to be set up
        DBManager man = new DBManager(false);
        man.recipesManager.GetRecipes();
        ViewData["RecipeList"] = man.recipesManager.recipes;
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
        DBManager man = new DBManager(false);
        string username = "david"; // change to cookies.username
        User david = man.userManager.getUserByUsername(username);
        man.recipesManager.GetRecipes();
        List<string> like = david.savedRecipes;
        List<Recipe>? recipes = man.recipesManager.GetRecipesIds(like);
        ViewData["RecipeList"] = recipes;
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

