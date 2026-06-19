using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTVISITORS")]
    public class Visitors
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        public string STUDENTID { get; set; } = string.Empty;
        public string NAME { get; set; } = string.Empty;
        public string RELATION { get; set; } = string.Empty;
        public string ADDRESS { get; set; } = string.Empty;
        public string MOBILE { get; set; } = string.Empty;
    }
}
