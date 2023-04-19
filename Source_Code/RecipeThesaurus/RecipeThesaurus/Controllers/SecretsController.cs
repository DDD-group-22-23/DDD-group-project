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
        //[Authorize] -- Causing errors
        // GET: /<controller>/
        public IActionResult Recpies()
        {
            return View();
        }
        public IActionResult Search()
        {
            return View();
        }
    }
}

