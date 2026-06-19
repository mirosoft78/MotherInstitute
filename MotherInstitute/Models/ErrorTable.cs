using System.ComponentModel.DataAnnotations;

namespace MotherInstitute.Models
{
    public class ErrorTable
    {
        [Key]
        public int SlNo { get; set; }

        public DateTime ErrorDate { get; set; }

        public string ErrorName { get; set; }

        public string ErrorMessage { get; set; }

        public string PageName { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string EventName { get; set; }

        public string StackTrace { get; set; }

        public string UserName { get; set; }

        public string OrganizationId { get; set; }

        public string IPAddress { get; set; }

        public string BrowserInfo { get; set; }
    }
}