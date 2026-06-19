using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MotherInstitute.Models
{
    public class RegistrationPageViewModel
    {
        public StudentRegd Student { get; set; } = new StudentRegd();

        public Visitors Visitor { get; set; } = new Visitors();
        public StudentFees Fee { get; set; } = new StudentFees();
        public StudentInstallment Installment { get; set; } = new StudentInstallment();
        public List<StudentSubjects> SubjectRows { get; set; } = new List<StudentSubjects>();
        public StudentSubjects Subject { get; set; } = new StudentSubjects();

        public List<Visitors> VisitorRows { get; set; } = new List<Visitors>();
        public List<StudentFees> FeeRows { get; set; } = new List<StudentFees>();
        public List<StudentInstallment> InstallmentRows { get; set; } = new List<StudentInstallment>();

        public IEnumerable<SelectListItem> SessionList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> CourseList { get; set; } = new List<SelectListItem>();
       
        public IEnumerable<SelectListItem> SubjectList { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> FeesList { get; set; } = new List<SelectListItem>();

        public string ActiveTab { get; set; } = "personal";
    }
}