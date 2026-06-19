using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MotherInstitute.Models;

namespace MotherInstitute.Controllers
{
    [Route("Marketing/MarketingPanel")]
    public class MarketingPanelController : Controller
    {
        private readonly AppDbContext _context;

        public MarketingPanelController(AppDbContext context)
        {
            _context = context;
        }

        // =====================================
        // VIEW STUDENT PAGE
        // =====================================

        [HttpGet("ViewStudent")]
        public IActionResult ViewStudent(
            int? schoolId,
            string district,
            string block,
            string studentName,
            string status)
        {
            var students = _context.MarketingStudents
                .Include(x => x.SchoolMaster)
                .AsQueryable();

            // SCHOOL FILTER

            if (schoolId != null)
            {
                students = students
                    .Where(x => x.SchoolId == schoolId);
            }

            // DISTRICT FILTER

            if (!string.IsNullOrEmpty(district))
            {
                students = students.Where(x =>
                    x.SchoolMaster != null &&
                    x.SchoolMaster.District != null &&
                    x.SchoolMaster.District
                        .Trim()
                        .ToLower() ==
                    district
                        .Trim()
                        .ToLower());
            }

            // BLOCK FILTER

            if (!string.IsNullOrEmpty(block))
            {
                students = students.Where(x =>
                    x.SchoolMaster != null &&
                    x.SchoolMaster.Block != null &&
                    x.SchoolMaster.Block
                        .Trim()
                        .ToLower() ==
                    block
                        .Trim()
                        .ToLower());
            }

            // STUDENT NAME FILTER

            if (!string.IsNullOrEmpty(studentName))
            {
                students = students
                    .Where(x =>
                        x.StudentName.Contains(studentName));
            }

            // STATUS FILTER

            if (!string.IsNullOrEmpty(status))
            {
                students = students
                    .Where(x => x.Status == status);
            }

            // VIEWBAG DATA

            ViewBag.Schools =
                _context.SchoolMasters.ToList();

            ViewBag.TotalStudents =
                _context.MarketingStudents.Count();

            ViewBag.VisitedStudents =
                _context.MarketingStudents
                .Count(x => x.Status == "Visited");

            ViewBag.InitiatedStudents =
                _context.MarketingStudents
                .Count(x => x.Status == "Initiated");

            return View(
                "VisitStudent",
                students
                    .OrderByDescending(x => x.ID)
                    .ToList()
            );
        }

        // =====================================
        // VIEW VISIT STUDENT PAGE
        // =====================================

        [HttpGet("ViewVisitStudent")]
        public IActionResult ViewVisitStudent(
            string searchText,
            bool isView = false)
        {
            ViewBag.SearchText = searchText;

            ViewBag.AllStudents =
                _context.MarketingStudents
                    .OrderBy(x => x.StudentName)
                    .ToList();

            // ALWAYS BLANK TEXTBOX

            ViewBag.StudentNote = "";

            var history =
                new List<ViewVisitHistoryVM>();

            if (isView)
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    var visits =
                        _context.MarketingVisit
                            .OrderByDescending(x => x.VisitDate)
                            .ToList();

                    history = visits
                        .Select(x =>
                            new ViewVisitHistoryVM
                            {
                                ID = x.ID,

                                AgentName = x.Agent,

                                VisitDate = x.VisitDate,

                                Remark = x.Notes
                            })
                        .ToList();
                }
                else
                {
                    var student =
                        _context.MarketingStudents
                            .FirstOrDefault(x =>
                                x.ID.ToString() == searchText ||
                                x.StudentName.Contains(searchText));

                    if (student != null)
                    {
                        var visits =
                            _context.MarketingVisit
                                .Where(x => x.ID == student.ID)
                                .OrderByDescending(x => x.VisitDate)
                                .ToList();

                        history = visits
                            .Select(x =>
                                new ViewVisitHistoryVM
                                {
                                    ID = x.ID,

                                    AgentName = x.Agent,

                                    VisitDate = x.VisitDate,

                                    Remark = x.Notes
                                })
                            .ToList();
                    }
                }
            }

            return View(history);
        }

        // =====================================
        // UPDATE STATUS
        // =====================================

        [HttpPost("UpdateStatus")]
        public IActionResult UpdateStatus(
            int id,
            string note)
        {
            try
            {
                if (id <= 0)
                {
                    return Redirect(
                        "/Marketing/MarketingPanel/ViewVisitStudent");
                }

                var student =
                    _context.MarketingStudents
                        .FirstOrDefault(x => x.ID == id);

                if (student == null)
                {
                    return Redirect(
                        "/Marketing/MarketingPanel/ViewVisitStudent");
                }

                // UPDATE STUDENT

                student.Note = note;

                student.Status = "Visited";

                student.VisitedDate = DateTime.Now;

                _context.MarketingStudents.Update(student);

                // GET LOGGED-IN MOBILE NUMBER

                string loggedInMobile =
                    HttpContext.Session
                        .GetString("UserName") ?? "";

                // SAVE VISIT HISTORY

                MarketingVisit visit =
                    new MarketingVisit()
                    {
                        ID = student.ID,

                        Agent = string.IsNullOrEmpty(loggedInMobile)
                            ? "Unknown"
                            : loggedInMobile,

                        VisitDate = DateTime.Now,

                        Notes = note
                    };

                _context.MarketingVisit.Add(visit);

                _context.SaveChanges();

                return Redirect(
                    "/Marketing/MarketingPanel/ViewVisitStudent" +
                    "?isView=true&searchText=" + id);
            }
            catch (Exception ex)
            {
                return Content(
                    ex.InnerException != null
                        ? ex.InnerException.Message
                        : ex.Message);
            }
        }

        // =====================================
        // UPDATE NOTE
        // =====================================

        [HttpPost("UpdateNote")]
        public IActionResult UpdateNote(
            int id,
            string note)
        {
            var student =
                _context.MarketingStudents
                    .FirstOrDefault(x => x.ID == id);

            if (student != null)
            {
                student.Note = note;

                _context.MarketingStudents.Update(student);

                _context.SaveChanges();
            }

            return Ok();
        }

        // =====================================
        // AGENT REPORT
        // =====================================

        [HttpGet("AgentWiseReport")]
        public IActionResult AgentWiseReport()
        {
            string agentName =
                HttpContext.Session
                    .GetString("PersonName");

            var data =
                _context.MarketingStudents
                    .Where(x => x.AgentName == agentName)
                    .GroupBy(x => x.AgentName)
                    .Select(x =>
                        new AgentReportViewModel
                        {
                            AgentName = x.Key,

                            TotalAssign = x.Count(),

                            TotalInitiated =
                                x.Count(a =>
                                    a.Status == "Initiated"),

                            TotalVisited =
                                x.Count(a =>
                                    a.Status == "Visited"),

                            TotalAdmitted =
                                x.Count(a =>
                                    a.Status == "Final"),

                            TotalCancel =
                                x.Count(a =>
                                    a.Status == "Cancel")
                        })
                    .ToList();

            return View(data);
        }
    }
}