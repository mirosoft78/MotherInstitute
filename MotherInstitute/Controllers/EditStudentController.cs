using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotherInstitute.Models;

namespace MotherInstitute.Controllers
{
    public class EditStudentController : Controller
    {
        private readonly AppDbContext _context;

        public EditStudentController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return RedirectToAction("Index", "SearchStudent");
            }

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == id);

            if (student == null)
            {
                return RedirectToAction("Index", "SearchStudent");
            }

            ViewBag.SessionList = _context.AcademicSessions
                .Select(x => new SelectListItem
                {
                    Value = x.NAME,
                    Text = x.NAME
                }).ToList();

            ViewBag.CourseList = _context.Courses
                .Select(x => new SelectListItem
                {
                    Value = x.NAME,
                    Text = x.NAME
                }).ToList();

            ViewBag.BedList = _context.BedDetailsList
                .Where(x => x.STATUS == "FREE" || x.BEDNO == student.BEDNO)
                .Select(x => new SelectListItem
                {
                    Value = x.BEDNO,
                    Text = x.BEDNO
                }).ToList();

            return View(student);
        }

        [HttpPost]
        public IActionResult Edit(StudentRegd model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.STUDENTID))
            {
                return RedirectToAction("Index", "SearchStudent");
            }

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == model.STUDENTID);

            if (student == null)
            {
                return RedirectToAction("Index", "SearchStudent");
            }

            var oldBedNo = student.BEDNO;
            var newBedNo = model.BEDNO;

            student.NAME = model.NAME;
            student.FNAME = model.FNAME;
            student.MNAME = model.MNAME;
            student.ADDRESS = model.ADDRESS;
            student.MOB1 = model.MOB1;
            student.MOB2 = model.MOB2;
            student.DOB = model.DOB;
            student.GENDER = model.GENDER;
            student.CASTE = model.CASTE;
            student.AADHARNO = model.AADHARNO;
            student.BLOODGROUP = model.BLOODGROUP;
            student.CURRYR = model.CURRYR;
            student.COURSE = model.COURSE;
            student.SESSION = model.SESSION;
            student.BEDNO = model.BEDNO;

            if (oldBedNo != newBedNo)
            {
                if (!string.IsNullOrWhiteSpace(oldBedNo))
                {
                    var oldBed = _context.BedDetailsList.FirstOrDefault(x => x.BEDNO == oldBedNo);
                    if (oldBed != null)
                    {
                        oldBed.STATUS = "FREE";
                    }
                }

                if (!string.IsNullOrWhiteSpace(newBedNo))
                {
                    var newBed = _context.BedDetailsList.FirstOrDefault(x => x.BEDNO == newBedNo);
                    if (newBed != null)
                    {
                        newBed.STATUS = "BOOKED";
                    }
                }
            }

            _context.SaveChanges();

            TempData["Message"] = "Student updated successfully.";
            return RedirectToAction("Index", "SearchStudent");
        }
    }
}