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

    public IActionResult Recommend()
    {
        // Not implemented
        return View();
    }

    public IActionResult Settings()
    {
        // Not implemented
        return View();
    }

    public IActionResult SaveRecipe()
    {
        DBManager man = new DBManager(false);
        User user = man.userManager.getUserByUsername("david"); // chnage to cookeis username
        int id = Convert.ToInt32(Request.Form["id"]);
        man.recipesManager.SaveRecipe(id, user);
        return RedirectToAction("Index");
    }

    public IActionResult UnsaveRecipe()
    {
        DBManager man = new DBManager(false);
        User user = man.userManager.getUserByUsername("david"); // chnage to cookeis username
        int id = Convert.ToInt32(Request.Form["id"]);
        man.recipesManager.UnsaveRecipe(id, user);
        return RedirectToAction("Index");
    }

    //can add as many likes as you want for now just to make it easier
    public IActionResult LikeRecipe()
    {
        DBManager man = new DBManager(false);
        //User user = man.userManager.getUserByUsername("david"); // chnage to cookeis username
        int id = Convert.ToInt32(Request.Form["id"]);
        man.recipesManager.LikeRecipe(id);
        return RedirectToAction("Index");
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
    public IActionResult CreateRecipe()
    {
        DBManager man = new DBManager(false);
        string title = Request.Form["title"];
        string description = Request.Form["description"];
        string ingredients = Request.Form["ingredients"];
        string instructions = Request.Form["instructions"];
        string image = Request.Form["image"];
        string username = "nikolai"; // Chnage to the cookie data username
        man.recipesManager.GetRecipes(); // Needed to get the next int for id
        man.recipesManager.CreateRecipe(title, description, ingredients, instructions, image, username);
        return RedirectToAction("Index");
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

