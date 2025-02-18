using System.ComponentModel.DataAnnotations;

namespace HealthCareSystem.Models.DTO
{
    public class RecommendationModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string Recommendations { get; set; } = string.Empty;
    }
}
