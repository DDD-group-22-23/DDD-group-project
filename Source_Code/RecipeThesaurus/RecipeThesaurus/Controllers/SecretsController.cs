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
            return View();
        }
        public IActionResult Search()
        {
            // DBManager needs to be set somewhere higher because its being recreated everywhere

            DBManager man = new DBManager(false);
            man.recipesManager.GetRecipes();
            string like = Request.Form["search"];
            List<Recipe>? recipes = man.recipesManager.GetRecipesLike(like);
            ViewData["Target"] = like;
            ViewData["List"] = recipes;
            return View();
        }
    }
}

