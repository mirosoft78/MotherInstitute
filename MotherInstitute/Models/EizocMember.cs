using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("EIZOCMEMBER")]
    public class EizocMember
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        public string? MID { get; set; }
        public string? NAME { get; set; }
        public string? EMAIL { get; set; }
        public string? MOBILE { get; set; }
        public string? ADDRESS { get; set; }
    }
}