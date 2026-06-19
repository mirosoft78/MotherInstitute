using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("COURSE")]
    public class Course
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        [Key]
        [Required(ErrorMessage = "Course name is required.")]
        [RegularExpression(@"^(?=.*[A-Za-z])[A-Za-z0-9+.\- ]+$",
            ErrorMessage = "Invalid course name.")]
        [StringLength(100)]
        public string NAME { get; set; } = string.Empty;
    }
}