using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTEXPENSES")]
    public class StudentExpense
    {
        [Key]
        public int SLNO { get; set; }   // Auto number

        [Required]
        public string STUDENTID { get; set; } = string.Empty;

        // 🔗 Link with StudentRegd
        [ForeignKey("STUDENTID")]
        public StudentRegd? Student { get; set; }

        [Required]
        public DateTime DATE { get; set; }

        [Required]
        public string CATEGORY { get; set; } = string.Empty;

        [Required]
        public string PARTICULARS { get; set; } = string.Empty;

        [Required]
        public decimal AMOUNT { get; set; }
    }
}