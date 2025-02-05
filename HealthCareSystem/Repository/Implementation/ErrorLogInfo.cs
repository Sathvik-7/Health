using HealthCareSystem.Context;
using HealthCareSystem.Models;
using HealthCareSystem.Repository.Interface;

namespace HealthCareSystem.Repository.Implementation
{
    public class ErrorLogInfo : IErrorLog
    {
        private readonly HealthCareContext _context;    

        public ErrorLogInfo(HealthCareContext healthCareContext)
        {
            this._context = healthCareContext;
        }

        void IErrorLog.insertError(string message,string stackTrace)
        {
            ErrorLog errorLogTable = new ErrorLog()
            { 
                ErrorMessage = message,
                StackTrace = stackTrace,
                CreatedAt = DateTime.Now
            };

            _context.ErrorLogs.Add(errorLogTable);
            _context.SaveChanges();
        }
    }
}
