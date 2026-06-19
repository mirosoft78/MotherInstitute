using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        // =========================================
        // LOGIN PAGE
        // =========================================

        [HttpGet]
        public IActionResult Index()
        {
            if (!string.IsNullOrEmpty(
                HttpContext.Session.GetString("UserName")))
            {
                return Redirect("/");
            }

            return View();
        }


        // =========================================
        // LOGIN SUBMIT
        // =========================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // LOGIN FROM USER TABLE

            var user = _context.Users.FirstOrDefault(x =>
                x.LOGINID == model.Username &&
                x.PSW == model.Password &&
                x.STATUS == "ACTIVE");

            if (user != null)
            {
                // SESSION

                HttpContext.Session.SetString(
                    "UserName",
                    user.LOGINID);

                HttpContext.Session.SetString(
                    "UserType",
                    user.TYPE ?? "");

                HttpContext.Session.SetString(
                    "PersonName",
                    user.NAME ?? "");


                // =====================================
                // ADMIN LOGIN
                // =====================================

                if (user.TYPE == "ADMIN")
                {
                    return Redirect("/Admin/Home");
                }


                // =====================================
                // EXECUTIVE LOGIN
                // =====================================

                if (user.TYPE == "EXECUTIVE")
                {
                    return Redirect("/Executive/Home");
                }


                // =====================================
                // MARKETING LOGIN
                // =====================================

                if (user.TYPE == "MARKETING")
                {
                    return Redirect("/Marketing/Home");
                }
            }

            // INVALID LOGIN

            ViewBag.ErrorMessage =
                "Invalid username or password";

            return View(model);
        }


        // =========================================
        // LOGOUT
        // =========================================

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return Redirect("/");
        }
    }
}