using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotherInstitute.Models
{
    [Table("STUDENTREGD")]
    public class StudentRegd
    {
        [Key]
        [Required]
        [StringLength(10)]
        public string STUDENTID { get; set; } = string.Empty;

        public string? SESSION { get; set; }
        public string? COURSE { get; set; }
        public string? BEDNO { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        public string? NAME { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        public string? FNAME { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(100)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        public string? MNAME { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string? MOB1 { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile number must be 10 digits.")]
        public string? MOB2 { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [RegularExpression(@"^[0-9]{12}$", ErrorMessage = "Aadhar number must be 12 digits.")]
        public string? AADHARNO { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        public string? ADDRESS { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        public DateTime? DOB { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        public DateTime? DOR { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        public string? GENDER { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Za-z ]+$", ErrorMessage = "Only letters allowed.")]
        public string? CASTE { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(10)]
        [RegularExpression(@"^[A-Za-z+-]+$", ErrorMessage = "Invalid blood group.")]
        public string? BLOODGROUP { get; set; }

        [StringLength(100)]
        public string? IMAGE { get; set; }

        [StringLength(200)]
        public string? COLLEGENAME { get; set; }

        [StringLength(200)]
        public string? BOARDNAME { get; set; }

        [StringLength(20)]
        public string? COLLEGEROLLNO { get; set; }

        [Required(ErrorMessage = "This value is required.")]
        [StringLength(50)]
        public string? CURRYR { get; set; }

        [NotMapped]
        public string? OLDSTUDENTID { get; set; }
    }
}