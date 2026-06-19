using System;
using System.Collections.Generic;

namespace MotherInstitute.Models
{
    public class EditStudentViewModel
    {
        public StudentRegd Student { get; set; } = new StudentRegd();

        public List<EditSubjectRow> SubjectRows { get; set; } = new();
        public List<EditFeeRow> FeeRows { get; set; } = new();
        public List<EditInstallmentRow> InstallmentRows { get; set; } = new();
        public List<EditVisitorRow> VisitorRows { get; set; } = new();
    }

    public class EditSubjectRow
    {
        public string SUBJECT { get; set; } = string.Empty;
    }

    public class EditFeeRow
    {
        public string FEESNAME { get; set; } = string.Empty;
        public decimal? AMOUNT { get; set; }
    }

    public class EditInstallmentRow
    {
        public string INSTALLMENTNAME { get; set; } = string.Empty;
        public decimal? AMOUNT { get; set; }
        public DateTime? DATE { get; set; }
        public string STATUS { get; set; } = "PENDING";
    }

    public class EditVisitorRow
    {
        public string NAME { get; set; } = string.Empty;
        public string RELATION { get; set; } = string.Empty;
        public string ADDRESS { get; set; } = string.Empty;
        public string MOBILE { get; set; } = string.Empty;
    }
}
