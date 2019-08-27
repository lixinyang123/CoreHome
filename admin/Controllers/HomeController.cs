using DataContext.DbOperator;
using DataContext.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace admin.Controllers
{
    public class HomeController : Controller
    {
        private ArticleRepository articleRepository;

        public HomeController()
        {
            articleRepository = new ArticleRepository();
        }

        public IActionResult Index()
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
