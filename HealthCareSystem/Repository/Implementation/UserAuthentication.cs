using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Identity;

namespace HealthCareSystem.Repository.Implementation
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly UserManager<IdentityUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IErrorLog _errorLog;

        public UserAuthentication(UserManager<IdentityUser> userManager, IErrorLog errorLog, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _errorLog = errorLog;
            _roleManager = roleManager;
        }   

        async Task<bool> IUserAuthentication.LoginUserAsync(LoginModel login)
        {
            var result = false;
            try
            {
                var userName = await _userManager.FindByEmailAsync(login.Email);

                if (userName != null)
                {
                    var passCheck = await _userManager.CheckPasswordAsync(userName, login.Password);
                
                    if(passCheck)
                        result = true;
                    else 
                        result = false;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }

            return result;
        }

        async Task<bool> IUserAuthentication.RegisterUserAsync(RegisterModel registerModel)
        {
            var result = false; 
            try
            {
                var userEmail = await _userManager.FindByEmailAsync(registerModel.Email);

                if (userEmail == null)
                {
                    var userInfo = new IdentityUser 
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email,
                        PhoneNumber = registerModel.PhoneNumber    
                    };
                    
                    var r = await _userManager.CreateAsync(userInfo, registerModel.Password);

                    if(r.Succeeded)
                    {
                       var exists = await _roleManager.RoleExistsAsync(registerModel.Role);

                        if(!exists)
                        {
                            var idenityRole = new IdentityRole(registerModel.Role);
                            
                            var succ = await _roleManager.CreateAsync(idenityRole);

                            return succ.Succeeded;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }
            return result;
        }
    }
}
