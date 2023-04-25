using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RecipeThesaurus.Controllers
{
    public class SecretsController : Controller
    {
        [Authorize]
        // GET: /<controller>/
        //public IActionResult Recpies()
        //{
            // Not implemented - To be removed? No need to look at individual recipes
        //    return View();
        //}
	    public IActionResult Index()
        {
            DBManager man = new DBManager(false);
            man.recipesManager.GetRecipes();
            ViewData["RecipeList"] = man.recipesManager.recipes;
            return View();
        }

        public IActionResult SaveRecipe()
        {
            DBManager man = new DBManager(false);
            string username = "david";
            User user = man.userManager.getUserByUsername(username); // chnage to cookeis username
            int id = Convert.ToInt32(Request.Form["id"]);
            man.recipesManager.SaveRecipe(id, user);
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

        public IActionResult Search()
        {
            // DBManager needs to be set somewhere higher because its being recreated everywhere

            DBManager man = new DBManager(false);
            man.recipesManager.GetRecipes();
            string like = Request.Form["search"];
            List<Recipe>? recipes = man.recipesManager.GetRecipesLike(like);
            ViewData["Target"] = like;
            ViewData["RecipeList"] = recipes;
            return View();
        }
    }
}

