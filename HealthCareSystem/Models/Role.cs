using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCareSystem.Models
{
    public class Role
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();

        public string RoleName { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        
        public RegisterUser RegisterUser { get; set; }
    }
}
