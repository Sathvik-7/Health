using HealthCareSystem.Context;
using HealthCareSystem.Repository.Implementation;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Unicode;
using Serilog;

namespace HealthCareSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddScoped<IUserAuthentication, UserAuthentication>();
            builder.Services.AddScoped<IErrorLog, ErrorLogInfo>();
            builder.Services.AddScoped<IPatientInformation,PatientInformation>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("Allow", policy =>
                {
                    policy.AllowAnyOrigin() // React app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<HealthCareContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("HealthCare"));
            });

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
            })
                .AddEntityFrameworkStores<HealthCareContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,

                        ValidAudience = builder.Configuration.GetSection("Jwt:Audience").Value,
                        ValidIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value,
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(builder.Configuration.GetSection("Jwt:Key").Value))
                    };

                });

            builder.Services.AddAuthorization();

            builder.Host.UseSerilog((context,configuration) => 
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            var app = builder.Build();
            //Cors
            app.UseCors("Allow");

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
