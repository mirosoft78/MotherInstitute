namespace MotherInstitute.Models
{
    public class ViewVisitHistoryVM
    {
        public int ID { get; set; }

        public string AgentName { get; set; }

        public DateTime VisitDate { get; set; }

        public string Remark { get; set; }
    }
}