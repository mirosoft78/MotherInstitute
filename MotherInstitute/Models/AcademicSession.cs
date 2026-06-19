using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("ACADEMICSESSION")]
    public class AcademicSession
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        [Key]

        [Required(ErrorMessage = "Session name is required.")]

        [RegularExpression(@"^\d{4}-\d{4}$",
            ErrorMessage = "Format should be YYYY-YYYY")]

        [StringLength(9)]
        public string NAME { get; set; } = string.Empty;
    }
}