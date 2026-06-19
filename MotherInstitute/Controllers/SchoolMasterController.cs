using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System.Linq;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class SchoolMasterController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public SchoolMasterController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        // INDEX

        public IActionResult Index()
        {
            var schools = _context.SchoolMasters
                                  .OrderByDescending(x => x.ID)
                                  .ToList();

            return View(schools);
        }

        // SAVE + UPDATE

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(SchoolMaster model)
        {
            try
            {
                if (model.ID == 0)
                {
                    _context.SchoolMasters.Add(model);

                    _context.SaveChanges();
                }
                else
                {
                    var existing = _context.SchoolMasters
                                           .FirstOrDefault(x => x.ID == model.ID);

                    if (existing != null)
                    {
                        existing.SchoolName = model.SchoolName;
                        existing.Address = model.Address;
                        existing.ContactNo = model.ContactNo;
                        existing.District = model.District;
                        existing.Block = model.Block;

                        _context.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "School Master",
    "SchoolMasterController",
    "Save",
    model.ID == 0 ? "Save School Button" : "Update School Button",
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

        // EDIT

        public IActionResult Edit(int id)
        {
            var school = _context.SchoolMasters
                                 .FirstOrDefault(x => x.ID == id);

            if (school == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.EditId = id;

            ViewBag.EditData = school;

            var schoolList = _context.SchoolMasters
                                     .OrderByDescending(x => x.ID)
                                     .ToList();

            return View("Index", schoolList);
        }

        // DELETE

        public IActionResult Delete(int id)
        {
            var school = _context.SchoolMasters
                                 .FirstOrDefault(x => x.ID == id);

            try
            {
                if (school != null)
                {
                    _context.SchoolMasters.Remove(school);

                    _context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
     ex.GetType().Name,
     ex.Message,
     "School Master",
     "SchoolMasterController",
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