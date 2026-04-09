using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Travel_Agent.Models;
using Travel_Agent.Models.DTO;

namespace Travel_Agent.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.HasIndex(u => u.EmployeeId).IsUnique();
                
                entity.Property(u => u.Level)
                    .HasConversion<string>()
                    .HasMaxLength(10);
                
                entity.Property(u => u.FirstName).HasMaxLength(100);
                entity.Property(u => u.LastName).HasMaxLength(100);
                entity.Property(u => u.EmployeeId).HasMaxLength(50);
                entity.Property(u => u.Subsidiary).HasMaxLength(200);
                entity.Property(u => u.Unit).HasMaxLength(200);
                entity.Property(u => u.LineManager).HasMaxLength(100);
                
            });
        }
    }
}