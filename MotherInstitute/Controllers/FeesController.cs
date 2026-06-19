using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class FeesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public FeesController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;

        }

        public IActionResult Index()
        {
            var fees = _context.Fees.ToList();
            return View(fees);
        }

        private bool IsValidFeesName(string name)
        {
            return Regex.IsMatch(name, @"^[A-Za-z ]+$");
        }

        [HttpPost]
        public IActionResult Create(Fees fees)
        {
            if (string.IsNullOrWhiteSpace(fees.NAME))
            {
                TempData["ErrorMessage"] = "Fees name is required.";
                return RedirectToAction("Index");
            }

            fees.NAME = fees.NAME.Trim();

            if (!IsValidFeesName(fees.NAME))
            {
                TempData["ErrorMessage"] = "Only letters allowed.";
                return RedirectToAction("Index");
            }

            var duplicate = _context.Fees
                .FirstOrDefault(x => x.NAME.ToLower() == fees.NAME.ToLower());

            if (duplicate != null)
            {
                TempData["ErrorMessage"] = "Fees already exists.";
                return RedirectToAction("Index");
            }

            try
            {
                _context.Fees.Add(fees);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Fees added successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Fees",
    "FeesController",
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

        [HttpPost]
        public JsonResult UpdateInline([FromBody] Fees model)
        {
            try
            {
                var existing = _context.Fees.FirstOrDefault(x => x.SLNO == model.SLNO);

                if (existing == null)
                {
                    return Json(new { success = false, message = "Fee not found" });
                }

                if (string.IsNullOrWhiteSpace(model.NAME))
                {
                    return Json(new { success = false, message = "Fee name is required" });
                }

                model.NAME = model.NAME.Trim();

                if (!IsValidFeesName(model.NAME))
                {
                    return Json(new { success = false, message = "Only letters allowed." });
                }

                if (existing.NAME.ToLower() == model.NAME.ToLower())
                {
                    return Json(new { success = true, message = "Fees updated successfully!" });
                }

                var duplicate = _context.Fees
                    .FirstOrDefault(x => x.NAME.ToLower() == model.NAME.ToLower());

                if (duplicate != null)
                {
                    return Json(new { success = false, message = "Fees already exists" });
                }

                _context.Fees.Remove(existing);
                _context.SaveChanges();

                Fees newFee = new Fees
                {
                    NAME = model.NAME
                };

                _context.Fees.Add(newFee);
                _context.SaveChanges();

                return Json(new { success = true, message = "Fees updated successfully!" });
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Fees",
    "FeesController",
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
            var fee = _context.Fees.FirstOrDefault(x => x.SLNO == id);

            try
            {
                if (fee != null)
                {
                    _context.Fees.Remove(fee);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Fees deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Fees not found.";
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Fees",
    "FeesController",
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