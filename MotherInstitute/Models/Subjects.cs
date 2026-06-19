using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("SUBJECTS")]
    public class Subjects
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        [Key]

        [Required(ErrorMessage = "Subject name is required.")]

        [RegularExpression(@"^[A-Za-z ]+$",
            ErrorMessage = "Only letters allowed.")]

        [StringLength(100)]
        public string NAME { get; set; } = string.Empty;
    }
}
