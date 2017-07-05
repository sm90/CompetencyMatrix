using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CompetencyMatrix.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApplicationCore.Data.Migrations;

namespace CompetencyMatrix.Data
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
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

        }


        const string adminRole = "admin";

        public static async Task InitializeDatabaseAsync(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            using (var db = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                await CreateAdminUser(context, serviceProvider);
            }
        }

        private static async Task CreateAdminUser(ApplicationDbContext context, IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleMgr.RoleExistsAsync(adminRole))
            {
                await roleMgr.CreateAsync(new IdentityRole(adminRole));
            }
            List<ApplicationUser> users = new List<ApplicationUser>();
            users.Add(new ApplicationUser { UserName = "devUser", Email = "dev@dev.com", PasswordHash = "Cool_123" });
            users.Add(new ApplicationUser { UserName = "workUser", Email = "work@work.com", PasswordHash = "Cool_123" });
            users.Add(new ApplicationUser { UserName = "admin", Email = "admin@work.com", PasswordHash = "Cool_123" });

            foreach (var u in users)
            {
                var userExist = await userMgr.FindByEmailAsync(u.Email);
                if (userExist == null)
                {
                    var userCreationResult = await userMgr.CreateAsync(u, u.PasswordHash);
                    context.Users.Add(u);
                    if (userCreationResult.Succeeded)
                    {
                        await userMgr.AddToRoleAsync(u, adminRole);
                    }
                    context.SaveChanges();
                }
            }
        }

        public static async Task AddUserToRole(ApplicationDbContext context, IServiceProvider serviceProvider,
            ApplicationUser user, string role)
        {
            var options = serviceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;
            var userMgr = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleMgr = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!await roleMgr.RoleExistsAsync(role))
            {
                await roleMgr.CreateAsync(new IdentityRole(role));
            }
            await userMgr.AddToRoleAsync(user, role);
            context.SaveChanges();
        }
    }
}
