using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Mohab Effat",
                    Email = "Mohab@Gmail.com",
                    UserName="Mohab_123",
                    Address = new Address
                    {
                        FirstName = "Mohab",
                        LastName = "Effat",
                        City = "El-Matarya",
                        State = "Cairo",
                        Street = "24",
                        PostalCode = "123456"
                    }
                };
                await userManager.CreateAsync(user, "Password123!");
            }
        }
    }
}
