using MotherInstitute.Models;

namespace MotherInstitute.Services
{
    public class ErrorLogger
    {
        private readonly AppDbContext _context;

        public ErrorLogger(AppDbContext context)
        {
            _context = context;
        }

        public void LogError(
            string errorName,
            string errorMessage,
            string pageName,
            string controllerName,
            string actionName,
            string eventName,
            string stackTrace,
            string userName,
            string organizationId,
            string ipAddress,
            string browserInfo)
        {
            ErrorTable error = new ErrorTable
            {
                ErrorDate = DateTime.Now,
                ErrorName = errorName,
                ErrorMessage = errorMessage,
                PageName = pageName,
                ControllerName = controllerName,
                ActionName = actionName,
                EventName = eventName,
                StackTrace = stackTrace,
                UserName = userName,
                OrganizationId = organizationId,
                IPAddress = ipAddress,
                BrowserInfo = browserInfo
            };

            _context.ErrorTable.Add(error);
            _context.SaveChanges();
        }
    }
}