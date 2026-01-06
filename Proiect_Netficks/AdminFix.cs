using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Proiect_Netficks.Models;
using System;
using System.Threading.Tasks;

namespace Proiect_Netficks.Data
{
    public static class AdminFix
    {
        public static async Task FixAdminAccount(IServiceProvider serviceProvider)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                    try
                    {
                        // Ensure Admin role exists
                        if (!await roleManager.RoleExistsAsync("Admin"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Admin"));
                        }

                        // Ensure Premium role exists
                        if (!await roleManager.RoleExistsAsync("Premium"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Premium"));
                        }

                        // Ensure Basic role exists
                        if (!await roleManager.RoleExistsAsync("Basic"))
                        {
                            await roleManager.CreateAsync(new IdentityRole("Basic"));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error creating roles: {ex.Message}");
                        // Continue execution
                    }

                    try
                    {
                        // Find admin user
                        var adminEmail = "admin@netficks.com";
                        var adminUser = await userManager.FindByEmailAsync(adminEmail);

                        if (adminUser != null)
                        {
                            // Ensure admin has Admin role
                            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
                            {
                                await userManager.AddToRoleAsync(adminUser, "Admin");
                            }

                            // Set Premium subscription type
                            adminUser.Tip_Abonament = "Premium";
                            await userManager.UpdateAsync(adminUser);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error updating admin user: {ex.Message}");
                        // Continue execution
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"General error in FixAdminAccount: {ex.Message}");
                // Let the application continue
            }
        }
    }
}