using Microsoft.EntityFrameworkCore;
using RoleAuthenticationApp.Models;

namespace RoleAuthenticationApp.Data
{
    public class RoleAuthDbContext : DbContext
    {
        public DbSet<Role> Roles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Строка подключения к SQL Server LocalDB
            optionsBuilder.UseSqlServer(@"Server=(localdb)\RoleAuthDB;Database=RoleAuthenticationDB;Trusted_Connection=true;TrustServerCertificate=true;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Настройка модели Role
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.USBSerial)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.HasIndex(e => e.USBSerial)
                    .IsUnique()
                    .HasDatabaseName("IX_Roles_USBSerial");

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("GETDATE()");
            });
        }
    }
}
