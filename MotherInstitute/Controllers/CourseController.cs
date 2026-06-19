using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class CourseController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public CourseController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }

        public IActionResult Index()
        {
            var courses = _context.Courses.ToList();
            return View(courses);
        }

        private bool IsValidCourseName(string name)
        {
            return Regex.IsMatch(name, @"^(?=.*[A-Za-z])[A-Za-z0-9+.\- ]+$");
        }

        [HttpPost]
        public IActionResult Create(Course course)
        {
            if (string.IsNullOrWhiteSpace(course.NAME))
            {
                TempData["ErrorMessage"] = "Course name is required.";
                return RedirectToAction("Index");
            }

            course.NAME = course.NAME.Trim();

            if (!IsValidCourseName(course.NAME))
            {
                TempData["ErrorMessage"] = "Invalid course name.";
                return RedirectToAction("Index");
            }

            var duplicate = _context.Courses
                .FirstOrDefault(x => x.NAME.ToLower() == course.NAME.ToLower());

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Course already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Courses.Add(course);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Course added successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
     ex.GetType().Name,
     ex.Message,
     "Course",
     "CourseController",
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
            var course = _context.Courses.FirstOrDefault(x => x.SLNO == id);

            if (course == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("Index");
            }

            return View(course);
        }

        [HttpPost]
        public IActionResult Edit(Course course)
        {
            var existingCourse = _context.Courses.FirstOrDefault(x => x.SLNO == course.SLNO);

            if (existingCourse == null)
            {
                TempData["ErrorMessage"] = "Course not found.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(course.NAME))
            {
                TempData["ErrorMessage"] = "Course name is required.";
                return RedirectToAction("Index");
            }

            course.NAME = course.NAME.Trim();

            if (!IsValidCourseName(course.NAME))
            {
                TempData["ErrorMessage"] = "Invalid course name.";
                return RedirectToAction("Index");
            }

            if (existingCourse.NAME == course.NAME)
            {
                TempData["SuccessMessage"] = "Course updated successfully!";
                return RedirectToAction("Index");
            }

            var duplicate = _context.Courses
                .FirstOrDefault(x => x.NAME.ToLower() == course.NAME.ToLower()
                                  && x.SLNO != course.SLNO);

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Course already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                existingCourse.NAME = course.NAME;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Course updated successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Course",
    "CourseController",
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
        public JsonResult UpdateInline([FromBody] Course model)
        {
            try
            {
                var existing = _context.Courses.FirstOrDefault(x => x.SLNO == model.SLNO);

                if (existing == null)
                {
                    return Json(new { success = false, message = "Course not found" });
                }

                if (string.IsNullOrWhiteSpace(model.NAME))
                {
                    return Json(new { success = false, message = "Course name is required" });
                }

                model.NAME = model.NAME.Trim();

                if (!IsValidCourseName(model.NAME))
                {
                    return Json(new { success = false, message = "Invalid course name" });
                }

                var duplicate = _context.Courses
                    .FirstOrDefault(x => x.NAME.ToLower() == model.NAME.ToLower()
                                      && x.SLNO != model.SLNO);

                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Course already exists" });
                }

                existing.NAME = model.NAME;

                _context.SaveChanges();

                return Json(new { success = true, message = "Course updated successfully!" });
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Course",
    "CourseController",
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
            var course = _context.Courses.FirstOrDefault(x => x.SLNO == id);

            try
            {
                if (course != null)
                {
                    _context.Courses.Remove(course);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Course deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Course not found.";
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Course",
    "CourseController",
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
    }
}