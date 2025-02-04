using HealthCareSystem.Context;
using HealthCareSystem.Models;
using HealthCareSystem.Repository.Interface;

namespace HealthCareSystem.Repository.Implementation
{
    public class ErrorLog : IErrorLog
    {
        private readonly HealthCareContext _context;    

        public ErrorLog(HealthCareContext healthCareContext)
        {
            this._context = healthCareContext;
        }

        void IErrorLog.insertError(string message,string stackTrace)
        {
            ErrorLogTable errorLogTable = new ErrorLogTable()
            { 
                ErrorMessage = message,
                StackTrace = stackTrace,
                CreatedAt = DateTime.Now
            };

            _context.errorLogTables.Add(errorLogTable);
        }
    }
}
