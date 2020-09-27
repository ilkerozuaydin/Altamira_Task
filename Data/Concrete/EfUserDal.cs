using Core.DataAccess.EntityFramework;
using Core.Entities.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfUserDal : EfEntityRepositoryBase<User, AltamiraContext>, IUserDal
    {
        public async Task<List<OperationClaim>> GetClaims(User user)
        {
            using (var context = new AltamiraContext())
            {
                var result = from operationClaim in context.OperationClaims
                             join userOperationClaim in context.UserOperationClaims
                                 on operationClaim.ID equals userOperationClaim.OperationClaimID
                             where userOperationClaim.UserID == user.ID
                             select new OperationClaim { ID = operationClaim.ID, Name = operationClaim.Name };
                return await result.ToListAsync();
            }
        }

        public async Task<List<User>> GetUsersWithIncludes()
        {
            using (var context = new AltamiraContext())
            {
                var result = await context.Users.Include(u => u.Address).Include(u => u.Company).Include(u=>u.Address.Geo).ToListAsync();
                return result;
            }
        }

        public async Task<User> GetUserWithIncludes(int userID)
        {
            using (var context = new AltamiraContext())
            {
                var result = await context.Users.Include(u => u.Address).Include(u => u.Company).Include(u => u.Address.Geo).FirstOrDefaultAsync(u=>u.ID == userID);
                return result;
            }
        }
    }
}