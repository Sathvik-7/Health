
using HealthCareSystem.Context;
using HealthCareSystem.Repository.Implementation;
using HealthCareSystem.Repository.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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

            builder.Services.AddScoped<IUserAuthentication,UserAuthentication>();
            builder.Services.AddScoped<IErrorLog,ErrorLog>();

            builder.Services.AddCors(options=> 
            {
                options.AddPolicy("Allow", policy =>
                {
                    policy.WithOrigins("http://localhost:5173") // React app URL
                   .AllowAnyHeader()
                   .AllowAnyMethod()
                   .AllowCredentials();
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

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
