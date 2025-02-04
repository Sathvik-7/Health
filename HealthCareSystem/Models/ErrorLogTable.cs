using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HealthCareSystem.Models
{
    public class ErrorLogTable
    {
        [Key]
        public int LogId { get; set; } 

        public string ErrorMessage { get; set; } = string.Empty;

        public string StackTrace {  get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}
