using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System.Linq;
using System.Collections.Generic;

namespace MotherInstitute.Controllers
{
    public class StudentStatusController : Controller
    {
        private readonly AppDbContext _context;

        public StudentStatusController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.Sessions = _context.StudentRegds
                .Select(x => x.SESSION)
                .Distinct()
                .ToList();

            ViewBag.Courses = _context.StudentRegds
                .Select(x => x.COURSE)
                .Distinct()
                .ToList();

            return View(new List<StudentRegd>());
        }

        [HttpPost]
        public IActionResult Index(string session, string course)
        {
            var students = _context.StudentRegds
                .Where(x => x.SESSION == session && x.COURSE == course)
                .ToList();

            ViewBag.Sessions = _context.StudentRegds
                .Select(x => x.SESSION)
                .Distinct()
                .ToList();

            ViewBag.Courses = _context.StudentRegds
                .Select(x => x.COURSE)
                .Distinct()
                .ToList();

            ViewBag.SelectedSession = session;
            ViewBag.SelectedCourse = course;

            return View(students);
        }

        [HttpPost]
        public IActionResult UpdateStatus(List<string> selectedIds, string newStatus, string session, string course)
        {
            if (selectedIds != null && !string.IsNullOrEmpty(newStatus))
            {
                foreach (var id in selectedIds)
                {
                    var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == id);
                    if (student != null)
                    {
                        student.CURRYR = newStatus;
                    }
                }

                _context.SaveChanges();
            }

            var students = _context.StudentRegds
                .Where(x => x.SESSION == session && x.COURSE == course)
                .ToList();

            ViewBag.Sessions = _context.StudentRegds
                .Select(x => x.SESSION)
                .Distinct()
                .ToList();

            ViewBag.Courses = _context.StudentRegds
                .Select(x => x.COURSE)
                .Distinct()
                .ToList();

            ViewBag.SelectedSession = session;
            ViewBag.SelectedCourse = course;
            ViewBag.Message = "Student status updated successfully.";

            return View("Index", students);
        }
    }
}
