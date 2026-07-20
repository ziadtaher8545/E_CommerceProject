using E_Commerce.Domain.Contracts;
using E_Commerce.Infrastructure.Identity.Data.Contexts;
using E_Commerce.Infrastructure.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.DataSeeders
{
    internal class IdentityDataSedder : IDataSeeder
    {
        private readonly StoreIdentityDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSedder> _logger;

        public IdentityDataSedder(StoreIdentityDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<IdentityDataSedder> logger)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this._logger = logger;
        }



        public async Task SeeedDataAsync(CancellationToken ct = default)
        {
            try
            {
                var pendingMigrations = context.Database.GetPendingMigrations();
                if (pendingMigrations.Any())
                    await context.Database.MigrateAsync(ct);
                if (!await roleManager.Roles.AnyAsync())
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                if (!await userManager.Users.AnyAsync())
                {
                    var admin = new ApplicationUser
                    {
                        DisplayName = "Ziad Taher",

                        Email = "ziad@gmail.com",
                        UserName = "ZiadTaher",
                        PhoneNumber = "01030650573"

                    };

                    var createResult = await userManager.CreateAsync(admin, "P@ssw0rd");
                    if (createResult.Succeeded)
                        await userManager.AddToRoleAsync(admin, "Admin");
                    else
                        _logger.LogWarning("Could not seed default admin user: {Errors}",
                            string.Join("; ", createResult.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Identity data seeding failed.");
                return;
            }

        }
    }
}
