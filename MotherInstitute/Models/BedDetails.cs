using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("BEDDETAILS")]
    public class BedDetails
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SLNO { get; set; }

        [Key]
        public string BEDNO { get; set; } = string.Empty;

        public string BUILDING { get; set; } = string.Empty;
        public string FLOOR { get; set; } = string.Empty;
        public string ROOM { get; set; } = string.Empty;
        public string STATUS { get; set; } = string.Empty;
    }
}
