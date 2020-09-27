using Core.Entities.Concrete;
using Core.Utilities.Security.Hashing;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework.SeedData
{
    public static class SeedDataMigrator
    {
        public static async Task GetSeedData()
        {
            using (var context = new AltamiraContext())
            {
                var isUserExists = await context.Users.AnyAsync();
                if (isUserExists)
                {
                    return;
                }

                using (var client = new HttpClient())
                {
                    var baseURL = new Uri("https://jsonplaceholder.typicode.com/users");
                    using (var response = await client.GetAsync(baseURL))
                    {
                        string data = response.Content.ReadAsStringAsync().Result;
                        var users = JsonConvert.DeserializeObject<List<User>>(data);
                        await SetSeedData(users, context);
                    }
                }
            }
        }

        private static async Task SetSeedData(List<User> users, AltamiraContext dbContext)
        {
            foreach (var user in users)
            {
                byte[] passwordHash, passwordSalt;
                HashingHelper.CreatePasswordHash("1234", out passwordHash, out passwordSalt);
                user.Password = passwordHash;
                user.PasswordSalt = passwordSalt;
                user.Status = true;
                user.ID = 0;
                user.UserOperationClaims.Add(new UserOperationClaim { UserID = user.ID, OperationClaimID = 1 });
            }

            dbContext.Users.AddRange(users);
            await dbContext.SaveChangesAsync();
        }
    }
}