using System;
using System.ComponentModel.DataAnnotations;

namespace MotherInstitute.Models
{
    public class MarketingAgent
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string MobileNo { get; set; } = string.Empty;

        public string? Address { get; set; }

        

        public DateTime JoiningDate { get; set; } = DateTime.Now;

        public string Status { get; set; } = "Active";
    }
}