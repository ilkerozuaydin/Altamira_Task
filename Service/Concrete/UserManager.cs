using Business.Abstract;
using Business.Constants;
using Core.CrossCuttingConcerns.Caching;
using Core.Entities.Concrete;
using Core.Utilities.Results;
using Core.Utilities.Security.Hashing;
using Core.Utilities.Security.Jwt;
using DataAccess.Abstract;
using Entities.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class UserManager : IUserService
    {
        private readonly IUserDal _userDal;
        private readonly ITokenHelper _tokenHelper;
        private readonly IUserOperationClaimDal _userOperationClaimDal;
        private readonly ICacheManager _cacheManager;

        public UserManager(IUserDal userDal, ITokenHelper tokenHelper, IUserOperationClaimDal userOperationClaimDal, ICacheManager cacheManager)
        {
            _userDal = userDal;
            _tokenHelper = tokenHelper;
            _userOperationClaimDal = userOperationClaimDal;
            _cacheManager = cacheManager;
        }

        public async Task<IResult> Delete(User user)
        {
            await _userDal.DeleteAsync(user);
            await _cacheManager.RemoveByPattern("GetUser*");
            return new SuccessResult(Messages.UserDeleted);
        }

        public async Task<IResult> Update(User user)
        {
            await _userDal.UpdateAsync(user);
            await _cacheManager.RemoveByPattern("GetUser*");
            return new SuccessResult(Messages.UserUpdated);
        }

        public async Task<IDataResult<User>> GetUser(int userID)
        {
            var user = await _cacheManager.Get<User>("GetUser?userID=" + userID.ToString());
            if (user != null)
            {
                return new SuccessDataResult<User>(user);
            }
            else
            {
                var response = new SuccessDataResult<User>(await _userDal.GetUserWithIncludes(userID));
                await _cacheManager.Add("GetUserList", response.Data, 10000);
                return response;
            }
        }

        public async Task<IDataResult<List<User>>> GetUserList()
        {
           var users = await _cacheManager.Get<List<User>>("GetUserList");
           if (users != null)
           {
               return new SuccessDataResult<List<User>>(users);
           }
           else
           {
               var response = new SuccessDataResult<List<User>>(await _userDal.GetUsersWithIncludes());
               await _cacheManager.Add("GetUserList", response.Data, 10000);
               return response;
           }
        }

        public async Task<IDataResult<List<OperationClaim>>> GetClaims(User user)
        {
            return new SuccessDataResult<List<OperationClaim>>(await _userDal.GetClaims(user));
        }

        public async Task<IDataResult<User>> GetByMail(string email)
        {
            return new SuccessDataResult<User>(await _userDal.GetAsync(u => u.Email == email));
        }

        public async Task<IDataResult<AccessToken>> CreateAccessToken(User user)
        {
            var claims = await GetClaims(user);
            if (claims.Success)
            {
                var accessToken = _tokenHelper.CreateToken(user, claims.Data);
                return new SuccessDataResult<AccessToken>(accessToken, Messages.AccessTokenCreated);
            }
            else
            {
                return new ErrorDataResult<AccessToken>(Messages.SystemError);
            }
        }

        public async Task<IDataResult<User>> Login(UserForLoginDto userForLoginDto)
        {
            var userToCheck = await GetByMail(userForLoginDto.Email);
            if (userToCheck.Data == null)
            {
                return new ErrorDataResult<User>(Messages.UserNotFound);
            }

            if (!HashingHelper.VerifyPasswordHash(userForLoginDto.Password, userToCheck.Data.Password, userToCheck.Data.PasswordSalt))
            {
                return new ErrorDataResult<User>(Messages.PasswordError);
            }

            return new SuccessDataResult<User>(userToCheck.Data, Messages.SuccessfulLogin);
        }

        public async Task<IResult> UserExists(string email)
        {
            var mailControl = await this.GetByMail(email);
            if (mailControl.Success)
            {
                if (mailControl.Data != null)
                {
                    return new ErrorResult(Messages.UserAlreadyExists);
                }
                return new SuccessResult();
            }
            else
            {
                return new ErrorResult();
            }
        }

        public async Task<IResult> Register(UserForRegisterDto userForRegisterDto)
        {
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userForRegisterDto.Password, out passwordHash, out passwordSalt);
            var user = new User
            {
                Email = userForRegisterDto.Email,
                Name = userForRegisterDto.Name,
                Password = passwordHash,
                PasswordSalt = passwordSalt,
                Status = true
            };
            await _userDal.AddAsync(user);
            await _cacheManager.RemoveByPattern("GetUser*");

            await AddUserOperationClaim(new UserOperationClaim { UserID = user.ID, OperationClaimID = 1 });
            return new SuccessResult(Messages.UserAdded);
        }

        public async Task<IResult> AddUserOperationClaim(UserOperationClaim userOperationClaim)
        {
            await _userOperationClaimDal.AddAsync(userOperationClaim);
            return new SuccessResult(Messages.UserAdded);
        }
    }
}