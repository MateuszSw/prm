using System.Collections.Generic;
using System.Linq;
using Prm.API.Models;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Prm.API.Data
{
    public class Seed
    {
        private readonly UserManager<User> _user;
        private readonly RoleManager<Role> _manager;

        public Seed(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _user = userManager;
            _manager = roleManager;
        }

        public void SeedUsers()
        {
            if (!_user.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                var roles = new List<Role>
                {
                    new Role{Name = "Admin"},
                    new Role{Name = "Teacher"},
                    new Role{Name = "Student"}
                    
                };

                foreach (var role in roles)
                {
                    _manager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    user.Photos.SingleOrDefault().Approved = true;
                    _user.CreateAsync(user, "password").Wait();
                    if(user.Status == "Student"){
                        _user.AddToRoleAsync(user, "Student").Wait();
                    }
                    _user.AddToRoleAsync(user, "Teacher").Wait();
                    
                }

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                IdentityResult score = _user.CreateAsync(adminUser, "password").Result;

                if (score.Succeeded)
                {
                    var admin = _user.FindByNameAsync("Admin").Result;
                    _user.AddToRolesAsync(admin, new[] {"Admin"}).Wait();
                }
            }
        }
    }
}