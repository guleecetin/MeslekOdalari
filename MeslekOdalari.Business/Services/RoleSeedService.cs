using MeslekOdalari.Entity.Entities.Enums;
using MeslekOdalari.Entity.Entities;
using Microsoft.AspNetCore.Identity;

namespace MeslekOdalari.Business.Services
{
    public class RoleSeedService : IRoleSeedService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleSeedService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task SeedRolesAsync()
        {
            var rolesToCreate = new Dictionary<string, string>
            {
                { UserRoles.Admin.ToString(), "Genel yönetici - tam yetki" },
                { UserRoles.Esnaf.ToString(), "Esnaf üyesi - esnaf işlemleri" },
                
            };

            foreach (var role in rolesToCreate)
            {
                if (!await _roleManager.RoleExistsAsync(role.Key))
                {
                    var newRole = new AppRole
                    {
                        Name = role.Key,
                        NormalizedName = role.Key.ToUpper()
                    };

                    var result = await _roleManager.CreateAsync(newRole);

                    if (result.Succeeded)
                    {
                        Console.WriteLine($"✓ {role.Key} rolü oluşturuldu");
                    }
                    else
                    {
                        Console.WriteLine($"✗ {role.Key} rolü oluşturulamadı");
                    }
                }
            }
        }
    }
}
