using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace HealthCareSystem.Models.DTO
{
    public class PatienDetailsModel
    {
        [Required]
        public Guid PatientId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

    }
}
