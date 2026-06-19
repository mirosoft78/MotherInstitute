using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class AcademicSessionController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public AcademicSessionController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        public IActionResult Index()
        {
            var sessions = _context.AcademicSessions.ToList();
            return View(sessions);
        }

        private bool IsValidSessionName(string name)
        {
            return Regex.IsMatch(name, @"^\d{4}-\d{4}$");
        }

        [HttpPost]
        public IActionResult Create(AcademicSession session)
        {
            if (string.IsNullOrWhiteSpace(session.NAME))
            {
                TempData["ErrorMessage"] = "Session name is .";
                return RedirectToAction("Index");
            }

            session.NAME = session.NAME.Trim();

            if (!IsValidSessionName(session.NAME))
            {
                TempData["ErrorMessage"] = "Invalid session format. Please enter like 2025-2026.";
                return RedirectToAction("Index");
            }

            var duplicateSession = _context.AcademicSessions.FirstOrDefault(x => x.NAME == session.NAME);

            if (duplicateSession != null)
            {
                TempData["ErrorMessage"] = "Session name already exists.";
                return RedirectToAction("Index");
            }

            try
            {

                _context.AcademicSessions.Add(session);
                
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Academic session saved successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
                    ex.GetType().Name,
                    ex.Message,
                    "Academic Session",
                    "AcademicSessionController",
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
            var session = _context.AcademicSessions.FirstOrDefault(x => x.SLNO == id);

            if (session == null)
            {
                return NotFound();
            }

            return View(session);
        }

        [HttpPost]
        public IActionResult Edit(AcademicSession session)
        {
            var existingSession = _context.AcademicSessions.FirstOrDefault(x => x.SLNO == session.SLNO);

            if (existingSession == null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(session.NAME))
            {
                TempData["ErrorMessage"] = "Session name is required.";
                return RedirectToAction("Index");
            }

            session.NAME = session.NAME.Trim();

            if (!IsValidSessionName(session.NAME))
            {
                TempData["ErrorMessage"] = "Invalid session format. Please enter like 2025-2026.";
                return RedirectToAction("Index");
            }

            var duplicateSession = _context.AcademicSessions
                .FirstOrDefault(x => x.NAME == session.NAME && x.SLNO != session.SLNO);

            if (duplicateSession != null)
            {
                TempData["ErrorMessage"] = "Session name already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                existingSession.NAME = session.NAME;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Academic session updated successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
                    ex.GetType().Name,
                    ex.Message,
                    "Academic Session",
                    "AcademicSessionController",
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
        public JsonResult UpdateInline([FromBody] AcademicSession session)
        {
            var existingSession = _context.AcademicSessions.FirstOrDefault(x => x.SLNO == session.SLNO);

            if (existingSession == null)
            {
                return Json(new { success = false, message = "Session not found" });
            }

            if (string.IsNullOrWhiteSpace(session.NAME))
            {
                return Json(new { success = false, message = "Session name is required" });
            }

            session.NAME = session.NAME.Trim();

            if (!IsValidSessionName(session.NAME))
            {
                return Json(new { success = false, message = "Invalid session format. Please enter like 2025-2026." });
            }

            try
            {
                var duplicateSession = _context.AcademicSessions
                    .FirstOrDefault(x => x.NAME == session.NAME && x.SLNO != session.SLNO);

                if (duplicateSession != null)
                {
                    return Json(new { success = false, message = "Session name already exists" });
                }

                existingSession.NAME = session.NAME;
                _context.SaveChanges();

                return Json(new { success = true, message = "Academic session updated successfully!" });
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
                    ex.GetType().Name,
                    ex.Message,
                    "Academic Session",
                    "AcademicSessionController",
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
            var session = _context.AcademicSessions.FirstOrDefault(x => x.SLNO == id);

            try
            {
                if (session != null)
                {
                    _context.AcademicSessions.Remove(session);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Academic session deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Session not found.";
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
                    ex.GetType().Name,
                    ex.Message,
                    "Academic Session",
                    "AcademicSessionController",
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