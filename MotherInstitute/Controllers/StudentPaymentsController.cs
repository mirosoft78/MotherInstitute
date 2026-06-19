using Microsoft.AspNetCore.Mvc;
using MotherInstitute.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MotherInstitute.Controllers
{
    public class StudentPaymentsController : Controller
    {
        private readonly AppDbContext _context;

        public StudentPaymentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(string studentId)
        {
            ViewBag.StudentId = studentId;

            if (string.IsNullOrWhiteSpace(studentId))
            {
                ViewBag.Expenses = new List<StudentExpense>();
                ViewBag.Installments = new List<StudentInstallment>();
                ViewBag.TotalExpense = 0;
                ViewBag.TotalPayment = 0;
                ViewBag.TotalInstallment = 0;
                ViewBag.DueAmount = 0;

                return View(new List<StudentPayment>());
            }

            var payments = _context.StudentPayments
                .Where(x => x.STUDENTID == studentId)
                .OrderByDescending(x => x.DATE)
                .ToList();

            var expenses = _context.StudentExpenses
                .Where(x => x.STUDENTID == studentId)
                .OrderByDescending(x => x.DATE)
                .ToList();

            var installments = _context.StudentInstallments
                .Where(x => x.STUDENTID == studentId)
                .OrderByDescending(x => x.DATE)
                .ToList();

            ViewBag.Expenses = expenses;
            ViewBag.Installments = installments;

            ViewBag.TotalExpense = expenses.Sum(x => x.AMOUNT);
            ViewBag.TotalPayment = payments.Sum(x => x.AMOUNT);
            ViewBag.TotalInstallment = installments.Sum(x => x.AMOUNT ?? 0);

            ViewBag.DueAmount =
                ViewBag.TotalExpense +
                ViewBag.TotalInstallment -
                ViewBag.TotalPayment;

            return View(payments);
        }

        [HttpGet]
        public IActionResult DownloadLedgerPdf(string studentId)
        {
            if (string.IsNullOrWhiteSpace(studentId))
            {
                return RedirectToAction("Index");
            }

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            var payments = _context.StudentPayments
                .Where(x => x.STUDENTID == studentId)
                .OrderBy(x => x.DATE)
                .ToList();

            var expenses = _context.StudentExpenses
                .Where(x => x.STUDENTID == studentId)
                .OrderBy(x => x.DATE)
                .ToList();

            var installments = _context.StudentInstallments
                .Where(x => x.STUDENTID == studentId)
                .OrderBy(x => x.DATE)
                .ToList();

            decimal totalExpense = expenses.Sum(x => Convert.ToDecimal(x.AMOUNT));
            decimal totalPayment = payments.Sum(x => Convert.ToDecimal(x.AMOUNT));
            decimal totalInstallment = installments.Sum(x => Convert.ToDecimal(x.AMOUNT ?? 0));
            decimal dueAmount = totalExpense + totalInstallment - totalPayment;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document(PageSize.A4, 35, 35, 25, 35);
                PdfWriter.GetInstance(doc, ms);
                doc.Open();

                Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, new BaseColor(0, 0, 0));
                Font subTitleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 13, new BaseColor(0, 0, 0));
                Font normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 10, new BaseColor(0, 0, 0));
                Font boldFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(0, 0, 0));
                Font whiteFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, new BaseColor(255, 255, 255));

                Paragraph institute = new Paragraph("THE MOTHER INSTITUTE OF SCIENCE", titleFont);
                institute.Alignment = Element.ALIGN_CENTER;
                doc.Add(institute);

                Paragraph reportTitle = new Paragraph("Student Account Ledger Report", subTitleFont);
                reportTitle.Alignment = Element.ALIGN_CENTER;
                reportTitle.SpacingAfter = 8;
                doc.Add(reportTitle);

                Paragraph generated = new Paragraph("Generated on: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm tt"), normalFont);
                generated.Alignment = Element.ALIGN_RIGHT;
                generated.SpacingAfter = 15;
                doc.Add(generated);

                PdfPTable infoTable = new PdfPTable(3);
                infoTable.WidthPercentage = 100;
                infoTable.SetWidths(new float[] { 33, 33, 34 });

                AddHeaderCell(infoTable, "Student Details", whiteFont);
                AddHeaderCell(infoTable, "Contact Information", whiteFont);
                AddHeaderCell(infoTable, "Academic Information", whiteFont);

                AddNormalCell(infoTable,
                    "Name: " + (student?.NAME ?? "N/A") + "\n" +
                    "Father: " + (student?.FNAME ?? "N/A") + "\n" +
                    "Admission ID: " + studentId, normalFont);

                AddNormalCell(infoTable,
                    "Mobile: " + (student?.MOB1 ?? "N/A") + "\n" +
                    "Address: " + (student?.ADDRESS ?? "N/A"), normalFont);

                AddNormalCell(infoTable,
                    "Course: " + (student?.COURSE ?? "N/A") + "\n" +
                    "Session: " + (student?.SESSION ?? "N/A"), normalFont);

                infoTable.SpacingAfter = 18;
                doc.Add(infoTable);

                PdfPTable summaryTable = new PdfPTable(4);
                summaryTable.WidthPercentage = 100;
                summaryTable.SetWidths(new float[] { 25, 25, 25, 25 });

                AddHeaderCell(summaryTable, "Total Expenses", whiteFont);
                AddHeaderCell(summaryTable, "Total Installments", whiteFont);
                AddHeaderCell(summaryTable, "Total Payments", whiteFont);
                AddHeaderCell(summaryTable, "Due Amount", whiteFont);

                AddNormalCenterCell(summaryTable, "Rs. " + totalExpense.ToString("0.00"), boldFont);
                AddNormalCenterCell(summaryTable, "Rs. " + totalInstallment.ToString("0.00"), boldFont);
                AddNormalCenterCell(summaryTable, "Rs. " + totalPayment.ToString("0.00"), boldFont);
                AddNormalCenterCell(summaryTable, "Rs. " + dueAmount.ToString("0.00"), boldFont);

                summaryTable.SpacingAfter = 20;
                doc.Add(summaryTable);

                Paragraph ledger = new Paragraph("Transaction Ledger", subTitleFont);
                ledger.Alignment = Element.ALIGN_CENTER;
                ledger.SpacingAfter = 10;
                doc.Add(ledger);

                PdfPTable ledgerTable = new PdfPTable(5);
                ledgerTable.WidthPercentage = 100;
                ledgerTable.SetWidths(new float[] { 10, 22, 25, 18, 25 });

                AddHeaderCell(ledgerTable, "SL", whiteFont);
                AddHeaderCell(ledgerTable, "Date", whiteFont);
                AddHeaderCell(ledgerTable, "Type", whiteFont);
                AddHeaderCell(ledgerTable, "MOP/Category", whiteFont);
                AddHeaderCell(ledgerTable, "Amount", whiteFont);

                int sl = 1;

                foreach (var e in expenses)
                {
                    AddNormalCenterCell(ledgerTable, sl++.ToString(), normalFont);
                    AddNormalCenterCell(ledgerTable, e.DATE.ToString("dd/MM/yyyy"), normalFont);
                    AddNormalCenterCell(ledgerTable, "Expense", normalFont);
                    AddNormalCenterCell(ledgerTable, e.CATEGORY ?? "-", normalFont);
                    AddNormalCenterCell(ledgerTable, "Rs. " + Convert.ToDecimal(e.AMOUNT).ToString("0.00"), normalFont);
                }

                foreach (var i in installments)
                {
                    AddNormalCenterCell(ledgerTable, sl++.ToString(), normalFont);
                    AddNormalCenterCell(ledgerTable, i.DATE?.ToString("dd/MM/yyyy") ?? "-", normalFont);
                    AddNormalCenterCell(ledgerTable, "Installment", normalFont);
                    AddNormalCenterCell(ledgerTable, "-", normalFont);
                    AddNormalCenterCell(ledgerTable, "Rs. " + Convert.ToDecimal(i.AMOUNT ?? 0).ToString("0.00"), normalFont);
                }

                foreach (var p in payments)
                {
                    AddNormalCenterCell(ledgerTable, sl++.ToString(), normalFont);
                    AddNormalCenterCell(ledgerTable, p.DATE.ToString("dd/MM/yyyy"), normalFont);
                    AddNormalCenterCell(ledgerTable, "Payment", normalFont);
                    AddNormalCenterCell(ledgerTable, p.MOP ?? "-", normalFont);
                    AddNormalCenterCell(ledgerTable, "Rs. " + Convert.ToDecimal(p.AMOUNT).ToString("0.00"), normalFont);
                }

                if (sl == 1)
                {
                    PdfPCell noData = new PdfPCell(new Phrase("No transactions recorded.", normalFont));
                    noData.Colspan = 5;
                    noData.HorizontalAlignment = Element.ALIGN_CENTER;
                    noData.Padding = 10;
                    ledgerTable.AddCell(noData);
                }

                ledgerTable.SpacingAfter = 35;
                doc.Add(ledgerTable);

                PdfPTable signTable = new PdfPTable(3);
                signTable.WidthPercentage = 100;

                AddSignatureCell(signTable, "Prepared By:\n\n____________________", normalFont);
                AddSignatureCell(signTable, "Checked By:\n\n____________________", normalFont);
                AddSignatureCell(signTable, "Approved By:\n\n____________________", normalFont);

                doc.Add(signTable);

                doc.Close();

                string fileName = "Account_Ledger_" + studentId.Replace("/", "_") + ".pdf";
                return File(ms.ToArray(), "application/pdf", fileName);
            }
        }

        private void AddHeaderCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.BackgroundColor = new BaseColor(37, 99, 235);
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 7;
            table.AddCell(cell);
        }

        private void AddNormalCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Padding = 8;
            table.AddCell(cell);
        }

        private void AddNormalCenterCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.Padding = 7;
            table.AddCell(cell);
        }

        private void AddSignatureCell(PdfPTable table, string text, Font font)
        {
            PdfPCell cell = new PdfPCell(new Phrase(text, font));
            cell.Border = Rectangle.NO_BORDER;
            cell.HorizontalAlignment = Element.ALIGN_CENTER;
            cell.PaddingTop = 20;
            table.AddCell(cell);
        }

        [HttpPost]
        public IActionResult Create(StudentPayment payment)
        {
            if (payment.DATE == default)
            {
                payment.DATE = DateTime.Now;
            }

            _context.StudentPayments.Add(payment);
            _context.SaveChanges();

            return RedirectToAction("Index", new { studentId = payment.STUDENTID });
        }

        [HttpPost]
        public JsonResult UpdatePayment([FromBody] StudentPayment payment)
        {
            var existing = _context.StudentPayments
                .FirstOrDefault(x => x.SLNO == payment.SLNO);

            if (existing != null)
            {
                existing.DATE = payment.DATE;
                existing.MOP = payment.MOP;
                existing.AMOUNT = payment.AMOUNT;

                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeletePayment(int id, string studentId)
        {
            var payment = _context.StudentPayments.FirstOrDefault(x => x.SLNO == id);

            if (payment != null)
            {
                _context.StudentPayments.Remove(payment);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { studentId = studentId });
        }

        [HttpPost]
        public JsonResult UpdateExpense([FromBody] StudentExpense expense)
        {
            var existing = _context.StudentExpenses
                .FirstOrDefault(x => x.SLNO == expense.SLNO);

            if (existing != null)
            {
                existing.DATE = expense.DATE;
                existing.CATEGORY = expense.CATEGORY;
                existing.AMOUNT = expense.AMOUNT;

                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteExpense(int id, string studentId)
        {
            var expense = _context.StudentExpenses.FirstOrDefault(x => x.SLNO == id);

            if (expense != null)
            {
                _context.StudentExpenses.Remove(expense);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { studentId = studentId });
        }

        [HttpPost]
        public JsonResult UpdateInstallment([FromBody] StudentInstallment installment)
        {
            var existing = _context.StudentInstallments
                .FirstOrDefault(x => x.SLNO == installment.SLNO);

            if (existing != null)
            {
                existing.DATE = installment.DATE;
                existing.AMOUNT = installment.AMOUNT;

                _context.SaveChanges();

                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        [HttpPost]
        public IActionResult DeleteInstallment(int id, string studentId)
        {
            var installment = _context.StudentInstallments.FirstOrDefault(x => x.SLNO == id);

            if (installment != null)
            {
                _context.StudentInstallments.Remove(installment);
                _context.SaveChanges();
            }

            return RedirectToAction("Index", new { studentId = studentId });
        }
    }
}