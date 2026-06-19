using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTFEE")]
    public class StudentFees
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }   // ✅ FIXED (was ID)

        public string? STUDENTID { get; set; }

        public string? FEESNAME { get; set; }

        public decimal? AMOUNT { get; set; }
    }
}



