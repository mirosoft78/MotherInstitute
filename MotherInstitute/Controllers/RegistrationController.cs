using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MotherInstitute.Models;
using System;
using System.IO;
using System.Linq;

namespace MotherInstitute.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly AppDbContext _context;

        public RegistrationController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(string activeTab = "personal", string? studentId = null)
        {
            var vm = new RegistrationPageViewModel
            {
                ActiveTab = activeTab
            };

            LoadDropdowns(vm);

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

                if (student != null)
                {
                    vm.Student = student;
                }
                else
                {
                    vm.Student.STUDENTID = studentId;
                }

                LoadStudentRelatedRows(vm, studentId);
            }

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterPersonal(RegistrationPageViewModel vm, IFormFile? StudentPhoto)
        {
            LoadDropdowns(vm);
            vm.ActiveTab = "personal";

            ClearOtherTabModelState();

            if (string.IsNullOrWhiteSpace(vm.Student.NAME))
                ModelState.AddModelError("Student.NAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.FNAME))
                ModelState.AddModelError("Student.FNAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MNAME))
                ModelState.AddModelError("Student.MNAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MOB1))
                ModelState.AddModelError("Student.MOB1", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MOB2))
                ModelState.AddModelError("Student.MOB2", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.AADHARNO))
                ModelState.AddModelError("Student.AADHARNO", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.ADDRESS))
                ModelState.AddModelError("Student.ADDRESS", "This value is required.");

            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            string selectedBedNo = vm.Student.BEDNO?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(selectedBedNo))
            {
                var bed = _context.BedDetailsList.FirstOrDefault(x => x.BEDNO == selectedBedNo);

                if (bed == null)
                {
                    TempData["ErrorMessage"] = "Selected bed number does not exist.";
                    return RedirectToAction("Index", new { activeTab = "personal" });
                }

                if (bed.STATUS == "BOOKED")
                {
                    TempData["ErrorMessage"] = "This bed is already BOOKED.";
                    return RedirectToAction("Index", new { activeTab = "personal" });
                }
            }

            string newStudentId = GenerateStudentId();
            string? imageFileName = null;

            if (StudentPhoto != null && StudentPhoto.Length > 0)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "studentphotos");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(StudentPhoto.FileName);
                string fullPath = Path.Combine(folderPath, imageFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    StudentPhoto.CopyTo(stream);
                }
            }

            var student = new StudentRegd
            {
                STUDENTID = newStudentId,
                SESSION = vm.Student.SESSION ?? "",
                COURSE = vm.Student.COURSE ?? "",
                BEDNO = selectedBedNo,
                NAME = vm.Student.NAME,
                FNAME = vm.Student.FNAME,
                MNAME = vm.Student.MNAME,
                MOB1 = vm.Student.MOB1,
                MOB2 = vm.Student.MOB2,
                DOB = vm.Student.DOB,
                DOR = vm.Student.DOR ?? DateTime.Now,
                GENDER = vm.Student.GENDER ?? "",
                CASTE = vm.Student.CASTE ?? "",
                AADHARNO = vm.Student.AADHARNO,
                BLOODGROUP = vm.Student.BLOODGROUP ?? "",
                ADDRESS = vm.Student.ADDRESS,
                IMAGE = imageFileName,
                COLLEGENAME = "",
                BOARDNAME = "",
                COLLEGEROLLNO = "",
                CURRYR = vm.Student.CURRYR ?? DateTime.Now.Year.ToString()
            };

            _context.StudentRegds.Add(student);
            _context.SaveChanges();

            if (!string.IsNullOrWhiteSpace(selectedBedNo))
            {
                var bed = _context.BedDetailsList.FirstOrDefault(x => x.BEDNO == selectedBedNo);

                if (bed != null)
                {
                    bed.STATUS = "BOOKED";
                    _context.SaveChanges();
                }
            }

            TempData["SuccessMessage"] = "Student registered successfully. Student ID: " + newStudentId;

            return RedirectToAction("Index", new
            {
                activeTab = "college",
                studentId = newStudentId
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdatePersonal(RegistrationPageViewModel vm, IFormFile? StudentPhoto)
        {
            LoadDropdowns(vm);
            vm.ActiveTab = "personal";

            ClearOtherTabModelState();

            if (string.IsNullOrWhiteSpace(vm.Student.NAME))
                ModelState.AddModelError("Student.NAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.FNAME))
                ModelState.AddModelError("Student.FNAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MNAME))
                ModelState.AddModelError("Student.MNAME", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MOB1))
                ModelState.AddModelError("Student.MOB1", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.MOB2))
                ModelState.AddModelError("Student.MOB2", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.AADHARNO))
                ModelState.AddModelError("Student.AADHARNO", "This value is required.");

            if (string.IsNullOrWhiteSpace(vm.Student.ADDRESS))
                ModelState.AddModelError("Student.ADDRESS", "This value is required.");

            if (!ModelState.IsValid)
            {
                return View("Index", vm);
            }

            if (string.IsNullOrWhiteSpace(vm.Student.STUDENTID))
            {
                TempData["ErrorMessage"] = "Student ID not found.";
                return RedirectToAction("Index", new { activeTab = "personal" });
            }

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == vm.Student.STUDENTID);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index", new { activeTab = "personal" });
            }

            string oldBedNo = student.BEDNO ?? "";
            string newBedNo = vm.Student.BEDNO?.Trim() ?? "";

            if (!string.IsNullOrWhiteSpace(newBedNo) && oldBedNo != newBedNo)
            {
                var newBed = _context.BedDetailsList.FirstOrDefault(x => x.BEDNO == newBedNo);

                if (newBed == null)
                {
                    TempData["ErrorMessage"] = "Selected bed number does not exist.";
                    return RedirectToAction("Index", new { activeTab = "personal", studentId = student.STUDENTID });
                }

                if (newBed.STATUS == "BOOKED")
                {
                    TempData["ErrorMessage"] = "This bed is already BOOKED.";
                    return RedirectToAction("Index", new { activeTab = "personal", studentId = student.STUDENTID });
                }
            }

            string? imageFileName = student.IMAGE;

            if (StudentPhoto != null && StudentPhoto.Length > 0)
            {
                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "studentphotos");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                imageFileName = Guid.NewGuid().ToString() + Path.GetExtension(StudentPhoto.FileName);
                string fullPath = Path.Combine(folderPath, imageFileName);

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    StudentPhoto.CopyTo(stream);
                }
            }

            student.NAME = vm.Student.NAME;
            student.FNAME = vm.Student.FNAME;
            student.MNAME = vm.Student.MNAME;
            student.MOB1 = vm.Student.MOB1;
            student.MOB2 = vm.Student.MOB2;
            student.ADDRESS = vm.Student.ADDRESS;
            student.AADHARNO = vm.Student.AADHARNO;
            student.DOB = vm.Student.DOB;
            student.BLOODGROUP = vm.Student.BLOODGROUP ?? "";
            student.SESSION = vm.Student.SESSION ?? "";
            student.COURSE = vm.Student.COURSE ?? "";
            student.GENDER = vm.Student.GENDER ?? "";
            student.CASTE = vm.Student.CASTE ?? "";
            student.DOR = vm.Student.DOR;
            student.BEDNO = newBedNo;
            student.CURRYR = vm.Student.CURRYR ?? student.CURRYR;
            student.IMAGE = imageFileName;

            if (!string.IsNullOrWhiteSpace(oldBedNo) && oldBedNo != newBedNo)
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

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Personal details updated successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "personal",
                studentId = student.STUDENTID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SaveCollege(RegistrationPageViewModel vm)
        {
            string studentId = vm.Student?.STUDENTID ?? "";

            if (string.IsNullOrWhiteSpace(studentId))
                studentId = Request.Form["Student.STUDENTID"].ToString();

            if (string.IsNullOrWhiteSpace(studentId))
                studentId = Request.Form["STUDENTID"].ToString();

            if (string.IsNullOrWhiteSpace(studentId))
            {
                TempData["ErrorMessage"] = "Student ID not found. First register the student from Personal tab.";
                return RedirectToAction("Index", new { activeTab = "college" });
            }

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            if (student == null)
            {
                TempData["ErrorMessage"] = "Student not found.";
                return RedirectToAction("Index", new { activeTab = "college", studentId });
            }

            student.BOARDNAME = vm.Student.BOARDNAME ?? "";
            student.COLLEGENAME = vm.Student.COLLEGENAME ?? "";
            student.COLLEGEROLLNO = vm.Student.COLLEGEROLLNO ?? "";

            _context.SaveChanges();

            TempData["SuccessMessage"] = "College details saved successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "visitors",
                studentId = student.STUDENTID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddVisitor(RegistrationPageViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Student.STUDENTID))
            {
                TempData["ErrorMessage"] = "Student ID not found. First register the student from Personal tab.";
                return RedirectToAction("Index", new { activeTab = "personal" });
            }

            if (!string.IsNullOrWhiteSpace(vm.Visitor.NAME))
            {
                vm.Visitor.STUDENTID = vm.Student.STUDENTID;
                _context.StudentVisitors.Add(vm.Visitor);
                _context.SaveChanges();

                TempData["SuccessMessage"] = "Visitor added successfully.";
            }

            return RedirectToAction("Index", new
            {
                activeTab = "visitors",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpGet]
        public IActionResult EditVisitor(int slno, string studentId)
        {
            var visitor = _context.StudentVisitors.FirstOrDefault(x => x.SLNO == slno);

            if (visitor == null)
            {
                TempData["ErrorMessage"] = "Visitor record not found.";
                return RedirectToAction("Index", new { activeTab = "visitors", studentId });
            }

            var vm = new RegistrationPageViewModel
            {
                ActiveTab = "visitors"
            };

            LoadDropdowns(vm);

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            if (student != null)
                vm.Student = student;
            else
                vm.Student.STUDENTID = studentId;

            vm.Visitor = visitor;
            LoadStudentRelatedRows(vm, studentId);

            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateVisitor(RegistrationPageViewModel vm)
        {
            var visitor = _context.StudentVisitors.FirstOrDefault(x => x.SLNO == vm.Visitor.SLNO);

            if (visitor == null)
            {
                TempData["ErrorMessage"] = "Visitor record not found.";
                return RedirectToAction("Index", new { activeTab = "visitors", studentId = vm.Student.STUDENTID });
            }

            visitor.NAME = vm.Visitor.NAME;
            visitor.MOBILE = vm.Visitor.MOBILE;
            visitor.RELATION = vm.Visitor.RELATION;
            visitor.ADDRESS = vm.Visitor.ADDRESS;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Visitor updated successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "visitors",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteVisitor(int slno, string studentId)
        {
            var visitor = _context.StudentVisitors.FirstOrDefault(x => x.SLNO == slno);

            if (visitor != null)
            {
                _context.StudentVisitors.Remove(visitor);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Visitor deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Visitor record not found.";
            }

            return RedirectToAction("Index", new { activeTab = "visitors", studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddSubject(RegistrationPageViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Student.STUDENTID))
            {
                TempData["ErrorMessage"] = "Student ID not found.";
                return RedirectToAction("Index", new { activeTab = "subjects" });
            }

            if (string.IsNullOrWhiteSpace(vm.Subject.SUBJECT))
            {
                TempData["ErrorMessage"] = "Please select subject.";
                return RedirectToAction("Index", new { activeTab = "subjects", studentId = vm.Student.STUDENTID });
            }

            var subject = new StudentSubjects
            {
                STUDENTID = vm.Student.STUDENTID,
                SUBJECT = vm.Subject.SUBJECT
            };

            _context.StudentSubjects.Add(subject);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Subject added successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "subjects",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpGet]
        public IActionResult EditSubject(int slno, string studentId)
        {
            var subject = _context.StudentSubjects.FirstOrDefault(x => x.SLNO == slno);

            if (subject == null)
            {
                TempData["ErrorMessage"] = "Subject record not found.";
                return RedirectToAction("Index", new { activeTab = "subjects", studentId });
            }

            var vm = new RegistrationPageViewModel
            {
                ActiveTab = "subjects"
            };

            LoadDropdowns(vm);

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            if (student != null)
                vm.Student = student;
            else
                vm.Student.STUDENTID = studentId;

            vm.Subject.SUBJECT = subject.SUBJECT;
            vm.Subject.SLNO = subject.SLNO;

            LoadStudentRelatedRows(vm, studentId);
            ViewBag.EditSubjectSlno = subject.SLNO;

            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateSubject(RegistrationPageViewModel vm)
        {
            var subjectRow = _context.StudentSubjects.FirstOrDefault(x => x.SLNO == vm.Subject.SLNO);

            if (subjectRow == null)
            {
                TempData["ErrorMessage"] = "Subject not found.";
                return RedirectToAction("Index", new { activeTab = "subjects", studentId = vm.Student.STUDENTID });
            }

            subjectRow.SUBJECT = vm.Subject.SUBJECT;
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Subject updated successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "subjects",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteSubject(int slno, string studentId)
        {
            var subject = _context.StudentSubjects.FirstOrDefault(x => x.SLNO == slno);

            if (subject != null)
            {
                _context.StudentSubjects.Remove(subject);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Subject deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Subject record not found.";
            }

            return RedirectToAction("Index", new { activeTab = "subjects", studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddFee(RegistrationPageViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Student.STUDENTID))
            {
                TempData["ErrorMessage"] = "Student ID not found.";
                return RedirectToAction("Index", new { activeTab = "fees", studentId = vm.Student.STUDENTID });
            }

            var fee = new StudentFees
            {
                STUDENTID = vm.Student.STUDENTID,
                FEESNAME = vm.Fee.FEESNAME,
                AMOUNT = vm.Fee.AMOUNT
            };

            _context.StudentFees.Add(fee);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Fee added successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "fees",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpGet]
        public IActionResult EditFee(int slno, string studentId)
        {
            var fee = _context.StudentFees.FirstOrDefault(x => x.SLNO == slno);

            if (fee == null)
            {
                TempData["ErrorMessage"] = "Fee record not found.";
                return RedirectToAction("Index", new { activeTab = "fees", studentId });
            }

            var vm = new RegistrationPageViewModel
            {
                ActiveTab = "fees"
            };

            LoadDropdowns(vm);

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            if (student != null)
                vm.Student = student;
            else
                vm.Student.STUDENTID = studentId;

            vm.Fee = fee;
            LoadStudentRelatedRows(vm, studentId);

            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateFee(RegistrationPageViewModel vm)
        {
            var fee = _context.StudentFees.FirstOrDefault(x => x.SLNO == vm.Fee.SLNO);

            if (fee == null)
            {
                TempData["ErrorMessage"] = "Fee record not found.";
                return RedirectToAction("Index", new { activeTab = "fees", studentId = vm.Student.STUDENTID });
            }

            fee.FEESNAME = vm.Fee.FEESNAME;
            fee.AMOUNT = vm.Fee.AMOUNT;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Fee updated successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "fees",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteFee(int slno, string studentId)
        {
            var fee = _context.StudentFees.FirstOrDefault(x => x.SLNO == slno);

            if (fee != null)
            {
                _context.StudentFees.Remove(fee);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Fee deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Fee record not found.";
            }

            return RedirectToAction("Index", new { activeTab = "fees", studentId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddInstallment(RegistrationPageViewModel vm)
        {
            if (string.IsNullOrWhiteSpace(vm.Student.STUDENTID))
            {
                TempData["ErrorMessage"] = "Student ID not found.";
                return RedirectToAction("Index", new { activeTab = "installments" });
            }

            if (vm.Installment.DATE == null)
            {
                TempData["ErrorMessage"] = "Please select installment date.";
                return RedirectToAction("Index", new { activeTab = "installments", studentId = vm.Student.STUDENTID });
            }

            if (vm.Installment.AMOUNT == null || vm.Installment.AMOUNT <= 0)
            {
                TempData["ErrorMessage"] = "Please enter amount.";
                return RedirectToAction("Index", new { activeTab = "installments", studentId = vm.Student.STUDENTID });
            }

            int nextNo = _context.StudentInstallments.Count(x => x.STUDENTID == vm.Student.STUDENTID) + 1;

            var installment = new StudentInstallment
            {
                STUDENTID = vm.Student.STUDENTID,
                DATE = vm.Installment.DATE,
                AMOUNT = vm.Installment.AMOUNT,
                STATUS = "Pending",
                INSTALLMENTNAME = "Installment " + nextNo
            };

            _context.StudentInstallments.Add(installment);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Installment added successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "installments",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpGet]
        public IActionResult EditInstallment(int id, string studentId)
        {
            var installment = _context.StudentInstallments.FirstOrDefault(x => x.SLNO == id);

            if (installment == null)
            {
                TempData["ErrorMessage"] = "Installment record not found.";
                return RedirectToAction("Index", new { activeTab = "installments", studentId });
            }

            var vm = new RegistrationPageViewModel
            {
                ActiveTab = "installments"
            };

            LoadDropdowns(vm);

            var student = _context.StudentRegds.FirstOrDefault(x => x.STUDENTID == studentId);

            if (student != null)
                vm.Student = student;
            else
                vm.Student.STUDENTID = studentId;

            vm.Installment = installment;
            LoadStudentRelatedRows(vm, studentId);

            return View("Index", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateInstallment(RegistrationPageViewModel vm)
        {
            var installment = _context.StudentInstallments.FirstOrDefault(x => x.SLNO == vm.Installment.SLNO);

            if (installment == null)
            {
                TempData["ErrorMessage"] = "Installment record not found.";
                return RedirectToAction("Index", new { activeTab = "installments", studentId = vm.Student.STUDENTID });
            }

            installment.DATE = vm.Installment.DATE;
            installment.AMOUNT = vm.Installment.AMOUNT;
            installment.STATUS = vm.Installment.STATUS;

            _context.SaveChanges();

            TempData["SuccessMessage"] = "Installment updated successfully.";

            return RedirectToAction("Index", new
            {
                activeTab = "installments",
                studentId = vm.Student.STUDENTID
            });
        }

        [HttpGet]
        public IActionResult DeleteInstallment(int id, string studentId)
        {
            var installment = _context.StudentInstallments.FirstOrDefault(x => x.SLNO == id);

            if (installment != null)
            {
                _context.StudentInstallments.Remove(installment);
                _context.SaveChanges();
                TempData["SuccessMessage"] = "Installment deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Installment record not found.";
            }

            return RedirectToAction("Index", new { activeTab = "installments", studentId });
        }

        private void ClearOtherTabModelState()
        {
            ModelState.Remove("Student.STUDENTID");

            ModelState.Remove("Visitor.SLNO");
            ModelState.Remove("Visitor.STUDENTID");
            ModelState.Remove("Visitor.NAME");
            ModelState.Remove("Visitor.MOBILE");
            ModelState.Remove("Visitor.RELATION");
            ModelState.Remove("Visitor.ADDRESS");

            ModelState.Remove("Subject.SLNO");
            ModelState.Remove("Subject.STUDENTID");
            ModelState.Remove("Subject.SUBJECT");

            ModelState.Remove("Fee.SLNO");
            ModelState.Remove("Fee.STUDENTID");
            ModelState.Remove("Fee.FEESNAME");
            ModelState.Remove("Fee.AMOUNT");

            ModelState.Remove("Installment.SLNO");
            ModelState.Remove("Installment.STUDENTID");
            ModelState.Remove("Installment.DATE");
            ModelState.Remove("Installment.AMOUNT");
            ModelState.Remove("Installment.STATUS");
            ModelState.Remove("Installment.INSTALLMENTNAME");
        }

        private void LoadStudentRelatedRows(RegistrationPageViewModel vm, string studentId)
        {
            vm.VisitorRows = _context.StudentVisitors.Where(x => x.STUDENTID == studentId).ToList();
            vm.SubjectRows = _context.StudentSubjects.Where(x => x.STUDENTID == studentId).ToList();
            vm.FeeRows = _context.StudentFees.Where(x => x.STUDENTID == studentId).ToList();
            vm.InstallmentRows = _context.StudentInstallments.Where(x => x.STUDENTID == studentId).ToList();
        }

        private void LoadDropdowns(RegistrationPageViewModel vm)
        {
            vm.SessionList = _context.AcademicSessions.Select(x => new SelectListItem
            {
                Value = x.NAME,
                Text = x.NAME
            }).ToList();

            vm.CourseList = _context.Courses.Select(x => new SelectListItem
            {
                Value = x.NAME,
                Text = x.NAME
            }).ToList();

            vm.SubjectList = _context.SubjectsList.Select(x => new SelectListItem
            {
                Value = x.NAME,
                Text = x.NAME
            }).ToList();

            vm.FeesList = _context.Fees.Select(x => new SelectListItem
            {
                Value = x.NAME,
                Text = x.NAME
            }).ToList();
        }

        private string GenerateStudentId()
        {
            var ids = _context.StudentRegds.Select(x => x.STUDENTID).ToList();

            int maxNo = 0;

            foreach (var id in ids)
            {
                if (!string.IsNullOrWhiteSpace(id) && id.StartsWith("TMIS/"))
                {
                    string numberPart = id.Replace("TMIS/", "");

                    if (int.TryParse(numberPart, out int number))
                    {
                        if (number > maxNo)
                            maxNo = number;
                    }
                }
            }

            string newId;
            int nextNo = maxNo + 1;

            do
            {
                newId = "TMIS/" + nextNo;
                nextNo++;
            }
            while (_context.StudentRegds.Any(x => x.STUDENTID == newId));

            return newId;
        }
    }
}