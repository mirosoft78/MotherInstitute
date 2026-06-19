using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MotherInstitute.Models;
using System;
using System.Linq;
using MotherInstitute.Services;

namespace MotherInstitute.Controllers
{
    public class MarketingStudentController : Controller
    {
        private readonly AppDbContext _context;
        private readonly ErrorLogger _errorLogger;

        public MarketingStudentController(AppDbContext context, ErrorLogger errorLogger)
        {
            _context = context;
            _errorLogger = errorLogger;
        }

        // INDEX

        public IActionResult Index()
        {
            LoadDropdowns();

            var data = _context.MarketingStudents
                               .Include(x => x.SchoolMaster)
                               .OrderByDescending(x => x.ID)
                               .ToList();

            return View(data);
        }

        // SAVE + UPDATE

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(MarketingStudent model)
        {
            if (model.ID == 0)
            {
                // INSERT

                model.InitiatedDate = DateTime.Now;

                // DEFAULT STATUS

                if (string.IsNullOrEmpty(model.Status))
                {
                    model.Status = "Initiated";
                }

                // VISITED DATE

                if (model.Status == "Visited" || model.Status == "Final")
                {
                    model.VisitedDate = DateTime.Now;
                }
                else
                {
                    model.VisitedDate = null;
                }

                _context.MarketingStudents.Add(model);
            }
            else
            {
                // UPDATE

                var existing = _context.MarketingStudents
                                       .FirstOrDefault(x => x.ID == model.ID);

                if (existing != null)
                {
                    existing.SchoolId = model.SchoolId;

                    existing.StudentName = model.StudentName;

                    existing.AgentName = model.AgentName;

                    existing.PhoneNumber = model.PhoneNumber;
                    existing.Session = model.Session;

                    existing.Address = model.Address;

                    existing.Note = model.Note;

                    // OLD STATUS

                    string oldStatus = existing.Status;

                    // UPDATE STATUS

                    existing.Status = model.Status;

                    // AUTO UPDATE VISITED DATE

                    if ((model.Status == "Visited" || model.Status == "Final")
                        && oldStatus != model.Status)
                    {
                        existing.VisitedDate = DateTime.Now;
                    }

                    // REMOVE VISITED DATE

                    if (model.Status == "Initiated" || model.Status == "Cancel")
                    {
                        existing.VisitedDate = null;
                    }

                    _context.MarketingStudents.Update(existing);
                }
            }

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Marketing Student",
    "MarketingStudentController",
    "Save",
    model.ID == 0 ? "Save Student Button" : "Update Student Button",
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
            var student = _context.MarketingStudents
                                  .FirstOrDefault(x => x.ID == id);

            if (student == null)
            {
                return RedirectToAction("Index");
            }

            ViewBag.EditData = student;

            LoadDropdowns();

            var data = _context.MarketingStudents
                               .Include(x => x.SchoolMaster)
                               .OrderByDescending(x => x.ID)
                               .ToList();

            return View("Index", data);
        }

        // DELETE

        public IActionResult Delete(int id)
        {
            var student = _context.MarketingStudents
                                  .FirstOrDefault(x => x.ID == id);

            if (student != null)
            {
                // =====================================
                // DELETE VISIT HISTORY FIRST
                // =====================================

                var visits = _context.MarketingVisit
                                     .Where(x => x.ID == id)
                                     .ToList();

                if (visits.Any())
                {
                    _context.MarketingVisit.RemoveRange(visits);
                }

                // =====================================
                // DELETE STUDENT
                // =====================================

                _context.MarketingStudents.Remove(student);

                try
                {
                    _context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _errorLogger.LogError(
    ex.GetType().Name,
    ex.Message,
    "Marketing Student",
    "MarketingStudentController",
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
            }

            return RedirectToAction("Index");
        }

        // DROPDOWNS

        private void LoadDropdowns()
        {
            ViewBag.Schools = new SelectList(
                _context.SchoolMasters.ToList(),
                "ID",
                "SchoolName"
            );

            ViewBag.Agents = new SelectList(
                _context.MarketingAgents.ToList(),
                "ID",
                "Name"
            );
        }
    }
}