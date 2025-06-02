using Microsoft.EntityFrameworkCore;
using UserManagement.API.Models.Domain;

namespace UserManagement.API.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        public DbSet<UserPermission> UserPermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // UserPermission → User
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.User)
                .WithMany(u => u.UserPermissions)
                .HasForeignKey(up => up.UserId)
                .OnDelete(DeleteBehavior.Restrict); 

            // UserPermission → Permission
            modelBuilder.Entity<UserPermission>()
                .HasOne(up => up.Permission)
                .WithMany(p => p.UserPermissions)
                .HasForeignKey(up => up.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);

            // Role → Permission
            modelBuilder.Entity<Role>()
                .HasOne(r => r.Permission)
                .WithMany()
                .HasForeignKey(r => r.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        }


    }
}
