using LostAndFound.Models;
using Microsoft.AspNetCore.Identity;

namespace LostAndFound.Utilities;

public static  class SeedData
{public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager< IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        await context.Database.EnsureCreatedAsync();
        string[] roleNames = { "Admin", "User" };
        foreach(var roleName in roleNames)
        {
            if(!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
        var adminEmail = "admin@lostandfound.com";
        var adminUser =await userManager.FindByEmailAsync(adminEmail);
if (adminUser ==null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "SysteM Administrator",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
        var userEmail = "user@lostandfound.com";
        var sampleUser = await userManager.FindByEmailAsync(userEmail);

        if (sampleUser==null)
        {
            sampleUser = new ApplicationUser
            {
                UserName = userEmail,
                Email = userEmail,
                FullName = "Sample User",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(sampleUser, "user@123");
            if(result.Succeeded)
            {
                await userManager.AddToRoleAsync(sampleUser, "User");
            }
        }




    }
}
