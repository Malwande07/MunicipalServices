using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MunicipalServices.Models;

namespace MunicipalServices.Controllers
{
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

/*
• Bootstrap. (2025) Bootstrap Documentation. Available at: https://getbootstrap.com/ (Accessed: 10 September 2025).
• File Upload in ASP.NET Core. (2025) Microsoft Docs – Upload files in ASP.NET Core. Available at: https://learn.microsoft.com/aspnet/core/mvc/models/file-uploads (Accessed: 10 September 2025).
• FontAwesome. (2025) FontAwesome Icons. Available at: https://fontawesome.com/ (Accessed: 10 September 2025).
• GitHub. (2025) GitHub – Code Hosting and Collaboration Platform. Available at: https://github.com/ (Accessed: 10 September 2025).
• HTML & CSS Tutorials. (2025) MDN Web Docs – HTML and CSS. Available at: https://developer.mozilla.org/en-US/docs/Web (Accessed: 10 September 2025).
• jQuery. (2025) jQuery API Documentation. Available at: https://api.jquery.com/ (Accessed: 10 September 2025).
• Razor Pages Documentation. (2025) Razor Pages in ASP.NET Core. Available at: https://learn.microsoft.com/aspnet/core/razor-pages/ (Accessed: 10 September 2025).
• Stack Overflow. (2025) Stack Overflow – Developer Community. Available at: https://stackoverflow.com/ (Accessed: 10 September 2025).
• W3Schools. (2025) W3Schools Online Web Tutorials. Available at: https://www.w3schools.com/ (Accessed: 10 September 2025).
*/
