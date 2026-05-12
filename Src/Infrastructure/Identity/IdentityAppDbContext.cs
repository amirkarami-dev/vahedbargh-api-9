using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Coreapi.Infrastructure.Identity
{
    public class IdentityAppDbContext(DbContextOptions<IdentityAppDbContext> options) : IdentityDbContext<ApplicationUser>(options)
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<ApplicationUser>()
            .HasIndex(b => b.IntegrateId)
            .IsUnique();

            //modelBuilder.Entity<IdentityRole>()
            //        .HasData(
            //        new IdentityRole("Executor"),
            //        new IdentityRole("Installer"),
            //        new IdentityRole("Accountant"),
            //        new IdentityRole("SuperUser"),
            //        new IdentityRole("Engineer"),
            //        new IdentityRole("Employee"),
            //        new IdentityRole("Administrator")
            //        );

            base.OnModelCreating(modelBuilder);

        }
    }
}
