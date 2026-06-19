using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using MotherInstitute.Services;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class OrganizationController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ErrorLogger _errorLogger;

        public OrganizationController(AppDbContext context, IWebHostEnvironment environment,
    ErrorLogger errorLogger)
        {
            _context = context;
            _environment = environment;
            _errorLogger = errorLogger;

        }

        [HttpGet]
        public IActionResult Index()
        {
            var org = new Organization();

            ViewBag.OrgList = _context.Organization
                                      .OrderBy(x => x.SLNO)
                                      .ToList();

            return View(org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Organization model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OrgList = _context.Organization
                                          .OrderBy(x => x.SLNO)
                                          .ToList();
                return View(model);
            }

            if (model.LogoFile != null && model.LogoFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.LogoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.LogoFile.CopyTo(stream);
                }

                model.LOGO = "/uploads/" + fileName;
            }

            try
            {
                _context.Organization.Add(model);
                _context.SaveChanges();

                TempData["Message"] = "Organization saved successfully";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Organization",
    "OrganizationController",
    "Index",
    "Submit Button",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var org = _context.Organization.FirstOrDefault(x => x.SLNO == id);

            if (org == null)
            {
                return NotFound();
            }

            ViewBag.OrgList = _context.Organization
                                      .OrderBy(x => x.SLNO)
                                      .ToList();

            return View("Index", org);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Organization model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.OrgList = _context.Organization
                                          .OrderBy(x => x.SLNO)
                                          .ToList();
                return View("Index", model);
            }

            var existingOrg = _context.Organization.FirstOrDefault(x => x.SLNO == model.SLNO);

            if (existingOrg == null)
            {
                return NotFound();
            }

            existingOrg.NAME = model.NAME;
            existingOrg.REGDNO = model.REGDNO;
            existingOrg.REGDDATE = model.REGDDATE;
            existingOrg.MOBILE = model.MOBILE;
            existingOrg.MAILID = model.MAILID;
            existingOrg.ADDRESS = model.ADDRESS;

            if (model.LogoFile != null && model.LogoFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(model.LogoFile.FileName);
                string filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.LogoFile.CopyTo(stream);
                }

                existingOrg.LOGO = "/uploads/" + fileName;
            }

            try
            {
                _context.SaveChanges();

                TempData["Message"] = "Organization updated successfully";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Organization",
    "OrganizationController",
    "Update",
    "Update Button",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var org = _context.Organization.FirstOrDefault(x => x.SLNO == id);

            try
            {
                if (org != null)
                {
                    _context.Organization.Remove(org);
                    _context.SaveChanges();

                    TempData["Message"] = "Organization deleted successfully";
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Organization",
    "OrganizationController",
    "Delete",
    "Delete Button",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("Index");
        }
    }
}