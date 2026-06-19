using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTPAYMENT")]
    public class StudentPayment
    {
        [Key]
        public int SLNO { get; set; }

        [Required]
        public string STUDENTID { get; set; } = string.Empty;

        [ForeignKey("STUDENTID")]
        public StudentRegd? Student { get; set; }

        [Required]
        public DateTime DATE { get; set; }

        [Required]
        public string MOP { get; set; } = string.Empty;

        [Required]
        public decimal AMOUNT { get; set; }
    }
}
