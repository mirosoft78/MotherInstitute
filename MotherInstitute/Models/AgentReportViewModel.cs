namespace MotherInstitute.Models
{
    public class AgentReportViewModel
    {
        public string AgentName { get; set; }

        public int TotalAssign { get; set; }

        public int TotalInitiated { get; set; }

        public int TotalVisited { get; set; }

        public int TotalAdmitted { get; set; }

        public int TotalCancel { get; set; }
    }
}