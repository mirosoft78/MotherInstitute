using Microsoft.AspNetCore.Mvc;

namespace MotherInstitute.Controllers
{
    public class MarketingController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
    }
}