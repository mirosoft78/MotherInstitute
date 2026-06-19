using System;
using System.ComponentModel.DataAnnotations;

namespace MotherInstitute.Models
{
    public class SchoolMaster
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string SchoolName { get; set; } = string.Empty;

        public string? Address { get; set; }

        public string? ContactNo { get; set; }
        public string? District { get; set; }

        public string? Block { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
