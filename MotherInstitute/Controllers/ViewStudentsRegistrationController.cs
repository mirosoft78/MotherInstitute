using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class ViewStudentsRegistrationController : Controller
    {
        private readonly AppDbContext _context;

        public ViewStudentsRegistrationController(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // VIEW STUDENTS PAGE LOAD
        // =====================================

        [HttpGet]
        public IActionResult Index()
        {
            // SESSION DROPDOWN

            ViewBag.Sessions = _context.StudentRegds
                                       .Select(x => x.SESSION)
                                       .Distinct()
                                       .ToList();

            // COURSE DROPDOWN

            ViewBag.Courses = _context.StudentRegds
                                      .Select(x => x.COURSE)
                                      .Distinct()
                                      .ToList();

            // STUDENT ID DROPDOWN

            ViewBag.StudentIds = _context.StudentRegds
                                         .Select(x => x.STUDENTID + " - " + x.NAME)
                                         .Distinct()
                                         .OrderBy(x => x)
                                         .ToList();

            // EMPTY GRID FIRST

            var data = new List<StudentRegd>();

            return View(data);
        }

        // =====================================
        // SEARCH
        // =====================================

        [HttpPost]
        public IActionResult Index(
            string studentId,
            string mobile,
            string session,
            string course,
            string year)
        {
            ViewBag.SelectedSession = session;

            // SESSION DROPDOWN

            ViewBag.Sessions = _context.StudentRegds
                                       .Select(x => x.SESSION)
                                       .Distinct()
                                       .ToList();

            // COURSE DROPDOWN

            ViewBag.Courses = _context.StudentRegds
                                      .Select(x => x.COURSE)
                                      .Distinct()
                                      .ToList();

            // STUDENT ID DROPDOWN

            ViewBag.StudentIds = _context.StudentRegds
                                         .Select(x => x.STUDENTID + " - " + x.NAME)
                                         .Distinct()
                                         .OrderBy(x => x)
                                         .ToList();

            // QUERY

            var query = _context.StudentRegds.AsQueryable();

            // STUDENT ID

            if (!string.IsNullOrEmpty(studentId))
            {
                query = query.Where(x =>
                    x.STUDENTID == studentId ||
                    (x.STUDENTID + " - " + x.NAME) == studentId);
            }

            // MOBILE

            if (!string.IsNullOrEmpty(mobile))
            {
                query = query.Where(x =>
                    x.MOB1.Contains(mobile) ||
                    x.MOB2.Contains(mobile));
            }

            // SESSION

            if (!string.IsNullOrEmpty(session))
            {
                query = query.Where(x =>
                    x.SESSION == session);
            }

            // COURSE

            if (!string.IsNullOrEmpty(course))
            {
                query = query.Where(x =>
                    x.COURSE == course);
            }

            // FINAL DATA

            var data = query
                       .OrderBy(x => x.STUDENTID)
                       .ToList();

            return View(data);
        }

        // =====================================
        // EDIT STUDENT REGISTRATION PAGE
        // =====================================

        [HttpGet]
        public IActionResult EditStudentRegistration()
        {
            // STUDENT ID DROPDOWN

            ViewBag.StudentIds = _context.StudentRegds
                                         .Select(x => x.STUDENTID + " - " + x.NAME)
                                         .Distinct()
                                         .OrderBy(x => x)
                                         .ToList();

            return View();
        }

        // =====================================
        // VIEW BUTTON CLICK
        // =====================================

        [HttpPost]
        public IActionResult EditStudentRegistration(string studentId)
        {
            // STUDENT ID DROPDOWN

            ViewBag.StudentIds = _context.StudentRegds
                                         .Select(x => x.STUDENTID + " - " + x.NAME)
                                         .Distinct()
                                         .OrderBy(x => x)
                                         .ToList();

            if (string.IsNullOrEmpty(studentId))
            {
                ViewBag.Message = "Please Enter Student ID";
                return View();
            }

            // REMOVE NAME PART

            if (studentId.Contains(" - "))
            {
                studentId = studentId.Split(" - ")[0];
            }

            var student = _context.StudentRegds
                                  .FirstOrDefault(x => x.STUDENTID == studentId);

            if (student == null)
            {
                ViewBag.Message = "Student Not Found";
                return View();
            }

            // REDIRECT TO MAIN REGISTRATION PAGE

            return RedirectToAction(
                "Index",
                "Registration",
                new
                {
                    studentId = student.STUDENTID
                });
        }
    }
}