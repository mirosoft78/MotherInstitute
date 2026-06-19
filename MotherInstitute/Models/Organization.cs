using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MotherInstitute.Models
{
    public class Organization
    {
        [Key]
        public int SLNO { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        [RegularExpression(@"^[A-Za-z .&]+$", ErrorMessage = "Only letters allowed.")]
        public string? NAME { get; set; }

        public string? LOGO { get; set; }

        [Required(ErrorMessage = "Regd No is required.")]
        public string? REGDNO { get; set; }

        [Required(ErrorMessage = "Regd Date is required.")]
        public DateTime? REGDDATE { get; set; }

        [Required(ErrorMessage = "Mobile is required.")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Mobile must be 10 digits.")]
        public string? MOBILE { get; set; }

        [Required(ErrorMessage = "Mail ID is required.")]
        [EmailAddress(ErrorMessage = "Enter valid email address.")]
        public string? MAILID { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public string? ADDRESS { get; set; }

        [NotMapped]
        public IFormFile? LogoFile { get; set; }
    }
}