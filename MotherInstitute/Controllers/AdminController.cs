using Microsoft.AspNetCore.Mvc;

namespace MotherInstitute.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}