using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("USER")]
    public class User
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        public DateTime REGDDATE { get; set; }

        [Key]
        public string LOGINID { get; set; } = string.Empty;

        public string NAME { get; set; } = string.Empty;

        public string PSW { get; set; } = string.Empty;

        public string TYPE { get; set; } = string.Empty;

        public string STATUS { get; set; } = string.Empty;
    }
}