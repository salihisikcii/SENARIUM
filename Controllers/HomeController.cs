using BlogApps.Controllers;
using BlogApps.Data.Abstract;
using BlogApps.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private IPostRepository _Postrepostitory;
        private ICategoriesRepository _CategoriesRepository;






        public HomeController(IPostRepository Postrepostitory, ICategoriesRepository CategoriesRepository)
        {
            _Postrepostitory = Postrepostitory;
            _CategoriesRepository = CategoriesRepository;

        }

        public IActionResult Index()
        {
            var posts = _Postrepostitory.Posts
                 .Include(p => p.categories)
                 .Include(p => p.User)
                 .Include(p => p.tags)
                 .ToList();




            return View(posts);
        }

       
        public IActionResult Hakkimda()
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
