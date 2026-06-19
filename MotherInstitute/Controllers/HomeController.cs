using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;

namespace MotherInstitute.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            ViewBag.ErrorMessage = "An unexpected error has occurred.";

            return View();
        }
    }
}