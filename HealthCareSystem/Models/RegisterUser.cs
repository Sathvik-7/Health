using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace HealthCareSystem.Models
{
    public class RegisterUser
    {
        [Key]
        public Guid UserId { get; set; } = Guid.NewGuid();

        public string UserName { get; set; } = string.Empty;

        public string Passsword { get; set; } = string.Empty;

        public string Email { get;set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //navigation property
        public ICollection<Role> Roles { get; set; } = new List<Role>();    

    }
}
