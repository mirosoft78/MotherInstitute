using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    public class MarketingStudent
    {
        [Key]
        public int ID { get; set; }

        public int SchoolId { get; set; }

        [ForeignKey("SchoolId")]
        public SchoolMaster? SchoolMaster { get; set; }

        public string? StudentName { get; set; }

        public string? AgentName { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string? Note { get; set; }

        public string? Status { get; set; }

        // NEW FIELD

        public int? Session { get; set; }

        public DateTime? InitiatedDate { get; set; }

        public DateTime? VisitedDate { get; set; }
    }
}