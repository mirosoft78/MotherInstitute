using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class StudentExpensesController : Controller
    {
        private readonly AppDbContext _context;

        public StudentExpensesController(AppDbContext context)
        {
            _context = context;
        }

        // PAGE LOAD

        [HttpGet]
        public IActionResult Index(string studentId)
        {
            ViewBag.StudentId = studentId;

            // STUDENT ID DROPDOWN

            ViewBag.StudentIds = _context.StudentRegds
                                         .Select(x => x.STUDENTID + " - " + x.NAME)
                                         .Distinct()
                                         .OrderBy(x => x)
                                         .ToList();

            // EXPENSES LIST

            var expenses = _context.StudentExpenses
                .Where(x => string.IsNullOrEmpty(studentId)
                         || x.STUDENTID == studentId)
                .OrderByDescending(x => x.DATE)
                .ToList();

            return View(expenses);
        }

        // CREATE

        [HttpPost]
        public IActionResult Create(StudentExpense expense)
        {
            // REMOVE NAME PART

            if (!string.IsNullOrEmpty(expense.STUDENTID))
            {
                expense.STUDENTID =
                    expense.STUDENTID.Split(" - ")[0];
            }

            // DATE AUTO

            if (expense.DATE == default)
            {
                expense.DATE = DateTime.Now;
            }

            _context.StudentExpenses.Add(expense);

            _context.SaveChanges();

            return RedirectToAction(
                "Index",
                new { studentId = expense.STUDENTID });
        }

        // UPDATE

        [HttpPost]
        public IActionResult UpdateExpense(
            [FromBody] StudentExpense expense)
        {
            var oldExpense = _context.StudentExpenses
                .FirstOrDefault(x => x.SLNO == expense.SLNO);

            if (oldExpense == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Expense record not found."
                });
            }

            // REMOVE NAME PART

            if (!string.IsNullOrEmpty(expense.STUDENTID))
            {
                expense.STUDENTID =
                    expense.STUDENTID.Split(" - ")[0];
            }

            oldExpense.STUDENTID = expense.STUDENTID;

            oldExpense.DATE = expense.DATE;

            oldExpense.CATEGORY = expense.CATEGORY;

            oldExpense.PARTICULARS = expense.PARTICULARS;

            oldExpense.AMOUNT = expense.AMOUNT;

            _context.SaveChanges();

            return Json(new
            {
                success = true
            });
        }

        // DELETE

        [HttpPost]
        public IActionResult DeleteExpense(
            int id,
            string studentId)
        {
            var expense = _context.StudentExpenses
                .FirstOrDefault(x => x.SLNO == id);

            if (expense != null)
            {
                _context.StudentExpenses.Remove(expense);

                _context.SaveChanges();
            }

            return RedirectToAction(
                "Index",
                new { studentId = studentId });
        }
    }
}