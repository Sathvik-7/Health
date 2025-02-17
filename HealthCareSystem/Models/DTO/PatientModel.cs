using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthCareSystem.Models.DTO
{
    public class PatientModel
    {
        //[Key]
        //public Guid PatientId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string ContactNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }
}
