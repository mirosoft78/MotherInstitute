using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;

namespace MotherInstitute.Controllers
{
    public class ExecutiveController : Controller
    {
        private readonly AppDbContext _context;

        public ExecutiveController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Home()
        {
            return View();
        }

        // REGISTRATION

        public IActionResult Registration()
        {
            return RedirectToAction("Index", "Registration");
        }

        // VIEW STUDENTS REGISTRATION

        public IActionResult ViewStudentsRegistration()
        {
            return RedirectToAction("Index", "ViewStudentsRegistration");
        }

        // EDIT STUDENT REGISTRATION

        public IActionResult EditStudentRegistration()
        {
            return RedirectToAction(
                "EditStudentRegistration",
                "ViewStudentsRegistration"
            );
        }

        // STUDENT STATUS

        public IActionResult StudentStatus()
        {
            return RedirectToAction("Index", "StudentStatus");
        }

        // =========================================
        // VIEW STUDENT VISIT
        // =========================================

        public IActionResult ViewStudentVisit(
            string studentId,
            string fromDate,
            string toDate,
            string showData)
        {
            var students = _context.MarketingStudents.ToList();

            var visitQuery = _context.MarketingVisit.AsQueryable();

            // ONLY AFTER CLICKING VIEW BUTTON

            if (!string.IsNullOrEmpty(showData))
            {
                // SEARCH BY STUDENT ID

                if (!string.IsNullOrEmpty(studentId))
                {
                    visitQuery = visitQuery
                        .Where(x => x.ID.ToString().Trim() == studentId.Trim());
                }

                // FROM DATE

                if (!string.IsNullOrEmpty(fromDate))
                {
                    DateTime from = Convert.ToDateTime(fromDate).Date;

                    visitQuery = visitQuery
                        .Where(x => x.VisitDate.Date >= from);
                }

                // TO DATE

                if (!string.IsNullOrEmpty(toDate))
                {
                    DateTime to = Convert.ToDateTime(toDate).Date;

                    visitQuery = visitQuery
                        .Where(x => x.VisitDate.Date <= to);
                }
            }
            else
            {
                // BEFORE CLICKING VIEW

                visitQuery = visitQuery.Where(x => false);
            }

            // FINAL DATA

            var visitList = visitQuery
                .OrderByDescending(x => x.VisitDate)
                .ToList()
                .Select(x => new
                {
                    x.SLNO,
                    x.ID,

                    StudentName = _context.MarketingStudents
                        .Where(s => s.ID == x.ID)
                        .Select(s => s.StudentName)
                        .FirstOrDefault(),

                    x.Agent,
                    x.VisitDate,
                    x.Notes
                })
                .ToList();

            ViewBag.VisitList = visitList;

            ViewBag.StudentId = studentId;

            ViewBag.FromDate = fromDate;

            ViewBag.ToDate = toDate;

            return View(students);
        }
        // Agent Visit History (total agent name will show here )
        [HttpGet]
        public IActionResult AgentVisitHistory()
        {
            var data = _context.MarketingStudents
                .GroupBy(x => x.AgentName)
                .Select(x => new AgentReportViewModel
                {
                    AgentName = x.Key,

                    TotalAssign = x.Count(),

                    TotalInitiated = x.Count(a =>
                        a.Status == "Initiated"),

                    TotalVisited = x.Count(a =>
                        a.Status == "Visited"),

                    TotalAdmitted = x.Count(a =>
                        a.Status == "Final"),

                    TotalCancel = x.Count(a =>
                        a.Status == "Cancel")
                })
                .ToList();

            return View(data);
        }
    }
}