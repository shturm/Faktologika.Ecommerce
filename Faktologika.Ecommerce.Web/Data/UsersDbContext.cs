using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Faktologika.Ecommerce.Web.Data;

class UsersDbContext : IdentityDbContext
{
    public UsersDbContext(DbContextOptions<UsersDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        string ADMIN_ID = "02174cf0–9412–4cfe-afbf-59f706d72cf6";
        string ROLE_ID = "341743f0-asd2–42de-afbf-59kmkkmk72cf6";

        //seed admin role
        builder.Entity<IdentityRole>().HasData(new IdentityRole
        {
            Name = "Admin",
            NormalizedName = "ADMIN",
            Id = ROLE_ID,
            ConcurrencyStamp = ROLE_ID
        });

        //create user
        var identityUser = new IdentityUser
        {
            Id = ADMIN_ID,
            Email = "admin@faktologika.bg",
            EmailConfirmed = true,
            // FirstName = "Frank",
            // LastName = "Ofoedu",
            UserName = "admin@faktologika.bg",
            NormalizedUserName = "ADMIN@FAKTOLOGIKA.BG"
        };

        //set user password
        PasswordHasher<IdentityUser> ph = new PasswordHasher<IdentityUser>();
        identityUser.PasswordHash = ph.HashPassword(identityUser, "qqz#123QWE");

        //seed user
        builder.Entity<IdentityUser>().HasData(identityUser);

        //set user role to admin
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
        {
            RoleId = ROLE_ID,
            UserId = ADMIN_ID
        });
    }
}
