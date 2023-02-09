using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using RecipeThesaurus.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeThesaurus.Pages
{
    public class IndexModel : PageModel
    { 
    /// <summary>  
    /// Gets or sets login model property.  
    /// </summary>  
    [BindProperty]
        public CoreLoginEfDbFirst.Models.LoginViewModel LoginModel { get; set; }
        #region Private Properties.  
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        /// <summary>  
        /// Database Manager property.  
        /// </summary>  

        #region Default Constructor method.  
        #region Public Properties  
        private readonly RecipeThesaurusContext databaseManager;
        private readonly ILogger _logger;
        /// <summary>  
        /// Initializes a new instance of the <see cref="IndexModel"/> class.  
        /// </summary>  
        /// <param name="databaseManagerContext">Database manager context parameter</param>  
        public IndexModel(RecipeThesaurusContext databaseManagerContext, ILogger<IndexModel> logger)
        {
            try
            {
                // Settings.  
                this.databaseManager = databaseManagerContext;
                _logger = logger;
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }
        }

        #endregion
    



    



        #endregion

        #region On Get method.  

        /// <summary>  
        /// GET: /Index  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public IActionResult OnGet()
        {
            try
            {
                // Verification.  
                if (this.User.Identity.IsAuthenticated)
                {
                    // Home Page.  
                    return this.RedirectToPage("/Home/Index");
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();
        }
        #endregion
        #region On Post Login method.  

        /// <summary>  
        /// POST: /Index/LogIn  
        /// </summary>  
        /// <returns>Returns - Appropriate page </returns>  
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                // Verification.  
                if (ModelState.IsValid)
                {
                    // Initialization.  

                    var loginInfo = await LoginByUsernamePassword.LoginByUsernamePasswordMethodAsync(this.LoginModel.Username, this.LoginModel.Password);
                    // Verification.  
                    if (loginInfo != null && loginInfo.Count() > 0)
                    {
                        // Initialization.  
                        var logindetails = loginInfo.First();

                        // Login In.  
                        await this.SignInUser(logindetails.Username, false);

                        // Info.  
                        return this.RedirectToPage("/Home/Index");
                    }
                    else
                    {
                        // Setting.  
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                // Info  
                Console.Write(ex);
            }

            // Info.  
            return this.Page();
        }

        #endregion

    }
    #endregion
}