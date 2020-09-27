using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Jwt;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IUserService
    {
        Task<IDataResult<List<User>>> GetUserList();

        Task<IDataResult<User>> GetUser(int userID);

        Task<IResult> Update(User user);

        Task<IResult> Delete(User user);

        Task<IDataResult<List<OperationClaim>>> GetClaims(User user);

        Task<IDataResult<User>> GetByMail(string email);

        Task<IDataResult<AccessToken>> CreateAccessToken(User user);

        Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto);

        Task<IResult> Register(UserForRegisterDto userForRegisterDto);

        Task<IResult> UserExists(string email);
    }
}