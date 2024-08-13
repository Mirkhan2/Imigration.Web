using Imigration.Application.Security;
using Imigration.Application.Services.Interfaces;
using Imigration.Application.Statics;
using Imigration.Domains.Entities.Account;
using Imigration.Domains.Interfaces;
using Imigration.Domains.ViewModels.Account;

namespace Imigration.Application.Services.Implementions
{
    public class UserService : IUserService
    {
        #region Ctor
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        #endregion

        #region Register


        public async Task<RegisterResult> RegisterUser(RegisterViewModel register)
        {
            if (await _userRepository.IsExistsUserByEmail(register.Email.Trim().ToLower()))
            {
                return RegisterResult.EmailExists;
            }
            var password = PasswordHelper.EncodePasswordMd5(register.Password);

            var user = new User
            {
                Avatar = PathTools.DefaultUserAvatar,
                Email = register.Email.Trim().ToLower(),
                Password = password,
                //EmailActivationCode = CodeGenerator.CreateActivationCode()

            };
            await _userRepository.CreateUser(user);
            await _userRepository.Save();

            #region Send Activitation Email

            #endregion
            return RegisterResult.Success;
        }


        #endregion
    }
}
