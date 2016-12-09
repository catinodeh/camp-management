using CampManagement.Web.Data;
using CampManagement.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CampManagement.Web
{
    public static class RolesData
    {
        private static readonly string[] Roles = new string[] { "Administrators", "Users" };

        public static async Task SeedRoles(IServiceProvider serviceProvider, string adminPWD)
        {
            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var db = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();

                var i = db.Users.Count();

                if (await db.Database.EnsureCreatedAsync() || true)
                {
                    var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();                    

                    if (!await roleManager.RoleExistsAsync("Administrators"))
                    {
                        foreach (var role in Roles)
                        {
                            if (!await roleManager.RoleExistsAsync(role))
                            {
                                await roleManager.CreateAsync(new IdentityRole(role));
                            }
                        }

                        var admin = new ApplicationUser()
                        {
                            UserName = "Administrator",
                            Email = "admin@admin.com"
                        };
                        await userManager.CreateAsync(admin, adminPWD);
                        await userManager.AddToRoleAsync(admin, "Administrators");
                    }
                }
            }
        }
    }
}
