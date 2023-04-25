using System.Diagnostics;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
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
        // DBManager needs to be set somewhere higher because its being recreated everywhere

        if (User.Claims.Count() > 3)
        {
            return RedirectToAction("Index", "Secrets", new { area = "Admin" });
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }





    public IActionResult Saved()
    {
        // DBManager needs to be set somewhere higher because its being recreated everywhere

        DBManager man = new DBManager(false);
        string username = "david"; // change to cookies.username
        User user = man.userManager.getUserByUsername(username);
        man.recipesManager.GetRecipes();
        List<string> like = user.savedRecipes;
        List<Recipe>? recipes = man.recipesManager.GetRecipesIds(like);
        ViewData["RecipeList"] = recipes;
        return View();
    }

    

        public IActionResult Create()
    {
        return View();
    }


    public IActionResult People()
    {
        // Not implemented
        return View();
    }

    public IActionResult Fridge()
    {
        // Not implemented
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

