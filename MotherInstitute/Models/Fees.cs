using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("FEES")]
    public class Fees
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        [Key]
        [Required(ErrorMessage = "Fees name is required.")]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        [StringLength(100)]
        public string NAME { get; set; } = string.Empty;
    }
}