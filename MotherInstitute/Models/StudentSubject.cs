using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTSUB")]
    public class StudentSubjects
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        public string STUDENTID { get; set; } = string.Empty;
        public string SUBJECT { get; set; } = string.Empty;
    }
}