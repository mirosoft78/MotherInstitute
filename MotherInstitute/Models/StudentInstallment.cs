using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTINSTALLMENT")]
    public class StudentInstallment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        public string? STUDENTID { get; set; }

        public string? INSTALLMENTNAME { get; set; }

        public DateTime? DATE { get; set; }

        public decimal? AMOUNT { get; set; }

        public string? STATUS { get; set; }
    }
}