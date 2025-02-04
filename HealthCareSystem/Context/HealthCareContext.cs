using HealthCareSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HealthCareSystem.Context
{
    public class HealthCareContext : IdentityDbContext
    {
        public HealthCareContext(DbContextOptions<HealthCareContext> options) : base(options) 
        { }

        public DbSet<ErrorLogTable> errorLogTables { get; set; }

    }
}
