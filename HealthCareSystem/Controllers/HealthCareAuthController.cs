using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace HealthCareSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCorsAttribute("Allow")]
    public class HealthCareAuthController : ControllerBase
    {
        private readonly IUserAuthentication userAuthentication;
        private readonly IErrorLog _errorLog;

        public HealthCareAuthController(IUserAuthentication userAuthentication,IErrorLog errorLog) 
        {
            this.userAuthentication = userAuthentication;
            _errorLog = errorLog;   
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login) 
        {
            var result = string.Empty;
            try 
            {
                result = await userAuthentication.LoginUserAsync(login);

                if (string.IsNullOrEmpty(result))
                    return NotFound(new { Message = "UserName/Password is in correct" });
            }
            catch (Exception ex) 
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }

            return Ok(new {
                            Message = "logged in successfully",
                            Token = result
                            });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            try
            {
                var result = await userAuthentication.RegisterUserAsync(registerModel);

                if (!result)
                    return NotFound(new { Message = "UserName & Email already exists." });
            }
            catch (Exception ex)
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }

            return Ok(new { Message = "Registered succcessfully" });
        }

    }
}
