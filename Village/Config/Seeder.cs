using Microsoft.AspNetCore.Identity;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Village.Common.Types;
using Village.Data.Domain;
using Village.Data.Identity;

namespace Village.Config
{
    public class RoleSeeder
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleSeeder(RoleManager<Role> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task Seed()
        {
            foreach (var role in Enum.GetNames(typeof(Roles)))
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new Role(role));
                }
            }
        }
    }

    public class UserSeeder
    {
        private readonly ApplicationUserManager _userManager;

        public UserSeeder(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task Seed()
        {
            var user = new User
            {
                UserName = "Elastique",
                Firstname = "Arjen",
                Lastname = "Bos",
                Email = "info@elastique.nl",
                EmailConfirmed = true
            };

            try
            {
                var result = await _userManager.CreateAsync(user, "Ad74djBorWHOR!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Roles.Administrator.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

    }
}
