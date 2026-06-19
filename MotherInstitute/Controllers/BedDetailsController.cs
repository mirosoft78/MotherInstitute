using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class BedDetailsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public BedDetailsController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        [HttpGet]
        public JsonResult CheckBedNo(string bedNo)
        {
            bool exists = _context.BedDetailsList.Any(x => x.BEDNO == bedNo);
            return Json(exists);
        }

        public IActionResult Index()
        {
            var beds = _context.BedDetailsList.ToList();
            return View(beds);
        }

        private bool IsValidBedNo(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z0-9\-]+$");
        }

        private bool IsValidBuilding(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z0-9 ]+$");
        }

        private bool IsValidFloor(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z0-9 ]+$");
        }

        private bool IsValidRoom(string value)
        {
            return Regex.IsMatch(value, @"^[A-Za-z0-9\-]+$");
        }

        [HttpPost]
        public IActionResult Create(BedDetails bed)
        {
            if (string.IsNullOrWhiteSpace(bed.BEDNO))
            {
                TempData["ErrorMessage"] = "Bed No is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.BUILDING))
            {
                TempData["ErrorMessage"] = "Building is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.FLOOR))
            {
                TempData["ErrorMessage"] = "Floor is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.ROOM))
            {
                TempData["ErrorMessage"] = "Room is required.";
                return RedirectToAction("Index");
            }

            bed.BEDNO = bed.BEDNO.Trim();
            bed.BUILDING = bed.BUILDING.Trim();
            bed.FLOOR = bed.FLOOR.Trim();
            bed.ROOM = bed.ROOM.Trim();

            if (!IsValidBedNo(bed.BEDNO))
            {
                TempData["ErrorMessage"] = "Invalid Bed No.";
                return RedirectToAction("Index");
            }

            if (!IsValidBuilding(bed.BUILDING))
            {
                TempData["ErrorMessage"] = "Invalid Building.";
                return RedirectToAction("Index");
            }

            if (!IsValidFloor(bed.FLOOR))
            {
                TempData["ErrorMessage"] = "Invalid Floor.";
                return RedirectToAction("Index");
            }

            if (!IsValidRoom(bed.ROOM))
            {
                TempData["ErrorMessage"] = "Invalid Room.";
                return RedirectToAction("Index");
            }

            var exists = _context.BedDetailsList.Any(x => x.BEDNO == bed.BEDNO);

            if (exists)
            {
                TempData["ErrorMessage"] = "This bed number already exists. Please add another.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.STATUS))
            {
                bed.STATUS = "FREE";
            }

            try
            {
                _context.BedDetailsList.Add(bed);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Bed added successfully!";
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Bed Details",
    "BedDetailsController",
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
            var bed = _context.BedDetailsList.FirstOrDefault(x => x.SLNO == id);

            if (bed == null)
            {
                TempData["ErrorMessage"] = "Bed not found.";
                return RedirectToAction("Index");
            }

            return View(bed);
        }

        [HttpPost]
        public IActionResult Edit(BedDetails bed)
        {
            var existing = _context.BedDetailsList.FirstOrDefault(x => x.SLNO == bed.SLNO);

            if (existing == null)
            {
                TempData["ErrorMessage"] = "Bed not found.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.BEDNO))
            {
                TempData["ErrorMessage"] = "Bed No is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.BUILDING))
            {
                TempData["ErrorMessage"] = "Building is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.FLOOR))
            {
                TempData["ErrorMessage"] = "Floor is required.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.ROOM))
            {
                TempData["ErrorMessage"] = "Room is required.";
                return RedirectToAction("Index");
            }

            bed.BEDNO = bed.BEDNO.Trim();
            bed.BUILDING = bed.BUILDING.Trim();
            bed.FLOOR = bed.FLOOR.Trim();
            bed.ROOM = bed.ROOM.Trim();

            if (!IsValidBedNo(bed.BEDNO))
            {
                TempData["ErrorMessage"] = "Invalid Bed No.";
                return RedirectToAction("Index");
            }

            if (!IsValidBuilding(bed.BUILDING))
            {
                TempData["ErrorMessage"] = "Invalid Building.";
                return RedirectToAction("Index");
            }

            if (!IsValidFloor(bed.FLOOR))
            {
                TempData["ErrorMessage"] = "Invalid Floor.";
                return RedirectToAction("Index");
            }

            if (!IsValidRoom(bed.ROOM))
            {
                TempData["ErrorMessage"] = "Invalid Room.";
                return RedirectToAction("Index");
            }

            if (string.IsNullOrWhiteSpace(bed.STATUS))
            {
                bed.STATUS = "FREE";
            }

            try
            {
                var duplicate = _context.BedDetailsList
                    .FirstOrDefault(x => x.BEDNO == bed.BEDNO && x.SLNO != bed.SLNO);

                if (duplicate != null)
                {
                    TempData["ErrorMessage"] = "This bed number already exists.";
                    return RedirectToAction("Index");
                }

                existing.BEDNO = bed.BEDNO;
                existing.BUILDING = bed.BUILDING;
                existing.FLOOR = bed.FLOOR;
                existing.ROOM = bed.ROOM;
                existing.STATUS = bed.STATUS;

                _context.SaveChanges();

                TempData["SuccessMessage"] = "Bed updated successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Bed Details",
    "BedDetailsController",
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
        }

        [HttpPost]
        public JsonResult UpdateInline([FromBody] BedDetails bed)
        {
            var existing = _context.BedDetailsList.FirstOrDefault(x => x.SLNO == bed.SLNO);

            if (existing == null)
            {
                return Json(new { success = false, message = "Bed not found" });
            }

            if (string.IsNullOrWhiteSpace(bed.BEDNO))
            {
                return Json(new { success = false, message = "Bed No is required" });
            }

            if (string.IsNullOrWhiteSpace(bed.BUILDING))
            {
                return Json(new { success = false, message = "Building is required" });
            }

            if (string.IsNullOrWhiteSpace(bed.FLOOR))
            {
                return Json(new { success = false, message = "Floor is required" });
            }

            if (string.IsNullOrWhiteSpace(bed.ROOM))
            {
                return Json(new { success = false, message = "Room is required" });
            }

            bed.BEDNO = bed.BEDNO.Trim();
            bed.BUILDING = bed.BUILDING.Trim();
            bed.FLOOR = bed.FLOOR.Trim();
            bed.ROOM = bed.ROOM.Trim();

            if (!IsValidBedNo(bed.BEDNO))
            {
                return Json(new { success = false, message = "Invalid Bed No" });
            }

            if (!IsValidBuilding(bed.BUILDING))
            {
                return Json(new { success = false, message = "Invalid Building" });
            }

            if (!IsValidFloor(bed.FLOOR))
            {
                return Json(new { success = false, message = "Invalid Floor" });
            }

            if (!IsValidRoom(bed.ROOM))
            {
                return Json(new { success = false, message = "Invalid Room" });
            }

            if (string.IsNullOrWhiteSpace(bed.STATUS))
            {
                bed.STATUS = "FREE";
            }

            try
            {
                var duplicate = _context.BedDetailsList
                    .FirstOrDefault(x => x.BEDNO == bed.BEDNO && x.SLNO != bed.SLNO);

                if (duplicate != null)
                {
                    return Json(new { success = false, message = "This bed number already exists" });
                }

                existing.BEDNO = bed.BEDNO;
                existing.BUILDING = bed.BUILDING;
                existing.FLOOR = bed.FLOOR;
                existing.ROOM = bed.ROOM;
                existing.STATUS = bed.STATUS;

                _context.SaveChanges();

                return Json(new { success = true, message = "Bed updated successfully!" });
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Bed Details",
    "BedDetailsController",
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
            var bed = _context.BedDetailsList.FirstOrDefault(x => x.SLNO == id);

            try
            {
                if (bed != null)
                {
                    _context.BedDetailsList.Remove(bed);
                    _context.SaveChanges();

                    TempData["SuccessMessage"] = "Bed deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Bed not found.";
                }
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Bed Details",
    "BedDetailsController",
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