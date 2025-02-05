using HealthCareSystem.Context;
using HealthCareSystem.Models;
using HealthCareSystem.Models.DTO;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace HealthCareSystem.Repository.Implementation
{
    public class UserAuthentication : IUserAuthentication
    {
        private readonly HealthCareContext _dbContext;
        private readonly IErrorLog _errorLog;
        private readonly IConfiguration _configuration;

        public UserAuthentication(IErrorLog errorLog, HealthCareContext dbContext, IConfiguration configuration)
        {
            _errorLog = errorLog;
            _dbContext = dbContext;
            _configuration = configuration;
        }

        async Task<string> IUserAuthentication.LoginUserAsync(LoginModel login)
        {
            var result = string.Empty;
            try
            {
                var userInfo = await _dbContext.RegisterUsers.FirstOrDefaultAsync(u => (u.Email == login.Email && u.Passsword == login.Password));

                if (userInfo != null)
                {
                    result = generateTokens(userInfo);
                }
            }
            catch (Exception ex)
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }

            return result;
        }
        
        private string generateTokens(RegisterUser userInfo)
        {
            var token = string.Empty;
            try
            {
                var Claims = new List<Claim>()
                {
                   new Claim(ClaimTypes.Email, userInfo.Email),
                };

                var userRoleInfo = _dbContext.Roles.Where(r => r.UserId == userInfo.UserId)
                                                .Select(r => r.RoleName)
                                                .ToList()
                                                .SelectMany(role => role.Contains(",")
                                                    ? role.Split(',').Select(r => r.Trim())  
                                                    : new[] { role })                        
                                                .ToList();

                // Add roles as claims
                foreach (var role in userRoleInfo)
                {
                    Claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

                var signCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

                var securityToken = new JwtSecurityToken(
                    issuer : _configuration.GetSection("Jwt:Issuer").Value,
                    audience : _configuration.GetSection("Jwt:Audience").Value,
                    expires : DateTime.UtcNow.AddMinutes(20),
                    claims : Claims,
                    signingCredentials : signCredentials
                    );

                token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            }
            catch (Exception ex)
            {
                _errorLog.insertError(ex.Message, ex.StackTrace);
            }

            return token;
        }

        async Task<bool> IUserAuthentication.RegisterUserAsync(RegisterModel registerModel)
        {
            var result = false;
            try
            {
                var response = await _dbContext.RegisterUsers
                                 .AnyAsync(u => (u.UserName == registerModel.UserName && u.Email == registerModel.Email));

                if (!response)
                {
                    var user = new RegisterUser()
                    {
                        UserName = registerModel.UserName,
                        Email = registerModel.Email,
                        Passsword = registerModel.Password
                    };

                    await _dbContext.RegisterUsers.AddAsync(user);
                    await _dbContext.SaveChangesAsync();

                    var role = new Role()
                    {
                        RoleName = string.Join(",", registerModel.Roles),
                        UserId = user.UserId
                    };

                    await _dbContext.Roles.AddAsync(role);
                    await _dbContext.SaveChangesAsync();

                    result = true;
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
