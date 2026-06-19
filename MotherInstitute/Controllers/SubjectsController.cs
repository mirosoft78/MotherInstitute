using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class SubjectsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public SubjectsController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        public IActionResult Index()
        {
            var subjects = _context.SubjectsList.ToList();
            return View(subjects);
        }

        private bool IsValidSubjectName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z ]+$");
        }

        [HttpPost]
        public IActionResult Create(Subjects subject)
        {
            if (string.IsNullOrWhiteSpace(subject.NAME))
            {
                TempData["ErrorMessage"] = "Subject name is required.";
                return RedirectToAction("Index");
            }

            subject.NAME = subject.NAME.Trim();

            if (!IsValidSubjectName(subject.NAME))
            {
                TempData["ErrorMessage"] = "Only letters allowed.";
                return RedirectToAction("Index");
            }

            var duplicate = _context.SubjectsList
                .FirstOrDefault(x => x.NAME.ToLower() == subject.NAME.ToLower());

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Subject already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.SubjectsList.Add(subject);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Subject added successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Subjects",
    "SubjectsController",
    "Create",
    "Add Button",
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

        public IActionResult Edit(int id)
        {
            var subject = _context.SubjectsList.FirstOrDefault(x => x.SLNO == id);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Subject not found.";
                return RedirectToAction("Index");
            }

            return View(subject);
        }

        [HttpPost]
        public IActionResult Edit(Subjects subject)
        {
            var existing = _context.SubjectsList.FirstOrDefault(x => x.SLNO == subject.SLNO);

            if (existing == null)
            {
                TempData["ErrorMessage"] = "Subject not found.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(subject.NAME))
            {
                TempData["ErrorMessage"] = "Subject name is required.";
                return RedirectToAction("Index");
            }

            subject.NAME = subject.NAME.Trim();

            if (!IsValidSubjectName(subject.NAME))
            {
                TempData["ErrorMessage"] = "Only letters allowed.";
                return RedirectToAction("Index");
            }

            var duplicate = _context.SubjectsList
                .FirstOrDefault(x =>
                    x.NAME.ToLower() == subject.NAME.ToLower()
                    && x.SLNO != subject.SLNO);

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Subject already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                existing.NAME = subject.NAME;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Subject updated successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Subjects",
    "SubjectsController",
    "Edit",
    "Edit Button",
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
        public JsonResult UpdateInline([FromBody] Subjects subject)
        {
            var existing = _context.SubjectsList.FirstOrDefault(x => x.SLNO == subject.SLNO);

            if (existing == null)
            {
                return Json(new { success = false, message = "Subject not found" });
            }

            if (string.IsNullOrWhiteSpace(subject.NAME))
            {
                return Json(new { success = false, message = "Subject name is required" });
            }

            subject.NAME = subject.NAME.Trim();

            if (!IsValidSubjectName(subject.NAME))
            {
                return Json(new { success = false, message = "Only letters allowed." });
            }

            try
            {
                var duplicate = _context.SubjectsList
                    .FirstOrDefault(x =>
                        x.NAME.ToLower() == subject.NAME.ToLower()
                        && x.SLNO != subject.SLNO);

                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Subject already exists" });
                }

                existing.NAME = subject.NAME;

                _context.SaveChanges();

                return Json(new { success = true, message = "Subject updated successfully!" });
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Subjects",
    "SubjectsController",
    "UpdateInline",
    "Inline Update",
    ex.StackTrace ?? "",
    User.Identity?.Name ?? "Guest",
    "",
    HttpContext.Connection.RemoteIpAddress?.ToString() ?? "",
    Request.Headers["User-Agent"].ToString()
);

                return Json(new
                {
                    success = false,
                    message = "An unexpected error has occurred."
                });
            }
        }

        [HttpPost]
        public IActionResult Delete(int id)
        {
            var subject = _context.SubjectsList.FirstOrDefault(x => x.SLNO == id);

            try
{
    if (subject != null)
    {
        _context.SubjectsList.Remove(subject);
        _context.SaveChanges();

        TempData["SuccessMessage"] = "Subject deleted successfully!";
    }
    else
    {
        TempData["ErrorMessage"] = "Subject not found.";
    }
}
catch (Exception ex)
{
                _errorLogger.LogError(
                ex.GetType().Name,
                ex.Message,
                "Subjects",
                "SubjectsController",
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