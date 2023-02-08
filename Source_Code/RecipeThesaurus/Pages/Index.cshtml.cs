using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RecipeThesaurus.Models.DB;

namespace RecipeThesaurus.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
#region Private Properties.  
  
/// <summary>  
/// Database Manager property.  
/// </summary>  
private readonly RecipeThesaurus databaseManager;
 
public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
#region Default Constructor method.  
  
        /// <summary>  
        /// Initializes a new instance of the <see cref="IndexModel"/> class.  
        /// </summary>  
        /// <param name="databaseManagerContext">Database manager context parameter</param>  
        public IndexModel(RecipeThesaurusContext databaseManagerContext)  
        {  
            try  
            {  
                // Settings.  
                this.databaseManager = databaseManagerContext;  
            }  
            catch (Exception ex)  
            {  
                // Info  
                Console.Write(ex);  
            }  
        }  
 
        #endregion

#region Public Properties

        /// <summary>
        /// Gets or sets login model property.
        /// </summary>
        [BindProperty]
        public LoginViewModel LoginModel { get; set; }

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
    }
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
            var loginInfo = await this.databaseManager.LoginByUsernamePasswordMethodAsync(this.LoginModel.Username, this.LoginModel.Password);  
  
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