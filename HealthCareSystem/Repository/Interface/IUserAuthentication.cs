using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;

namespace HealthCareSystem.Repository.Interface
{
    public interface IUserAuthentication
    {
        Task<string> LoginUserAsync(LoginModel login);

        Task<bool> RegisterUserAsync(RegisterModel registerModel);
    }
}
