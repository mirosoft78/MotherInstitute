using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    public class MarketingVisit
    {
        [Key]
        public int SLNO { get; set; }

        public int ID { get; set; }

        // NOW SAVE AGENT NAME

        public string? Agent { get; set; }

        public DateTime VisitDate { get; set; }

        public string? Notes { get; set; }

        [ForeignKey("ID")]
        public MarketingStudent? MarketingStudent { get; set; }
    }
}