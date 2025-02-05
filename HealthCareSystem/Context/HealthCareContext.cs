using HealthCareSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSystem.Context
{
    public class HealthCareContext : DbContext
    {
        public HealthCareContext(DbContextOptions<HealthCareContext> options) : base(options) 
        { }

        public DbSet<ErrorLog> ErrorLogs { get; set; }

        public DbSet<RegisterUser> RegisterUsers { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
           .HasOne(r => r.RegisterUser)
           .WithMany(u => u.Roles)
           .HasForeignKey(r => r.UserId)
           .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
