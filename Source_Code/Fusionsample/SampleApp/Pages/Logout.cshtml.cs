using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace SampleApp.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly ILogger<LogoutModel> _logger;
	private readonly IConfiguration _configuration;

        public LogoutModel(ILogger<LogoutModel> logger, IConfiguration configuration)
        {
            _logger = logger;
	    _configuration = configuration;
        }

	public IActionResult OnGet()
	{
              SignOut("cookie", "oidc");
              var host = _configuration["SampleApp:Authority"];
              var cookieName = _configuration["SampleApp:CookieName"];

              var clientId = _configuration["SampleApp:ClientId"];
              var url = host + "/oauth2/logout?client_id="+clientId;
              Response.Cookies.Delete(cookieName);
              return Redirect(url);
        }
    }
}
