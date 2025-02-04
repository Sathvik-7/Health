using System.Runtime.CompilerServices;

namespace HealthCareSystem.Repository.Interface
{
    public interface IErrorLog
    {
        void insertError(string message, string stackTrace);
    }
}
