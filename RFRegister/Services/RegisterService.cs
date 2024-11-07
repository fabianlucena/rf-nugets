using AutoMapper;
using RFAuth.Entities;
using RFAuth.IServices;
using RFUserEmail.Entities;
using RFUserEmail.IServices;
using RFRegister.DTO;
using RFRegister.IServices;

namespace RFRegister.Services
{
    public class RegisterService(IUserService userService, IPasswordService passwordService, IUserEmailService userEmailService, IMapper mapper) : IRegisterService
    {
        public async Task RegisterAsync(RegisterRequest registerData)
        {
            if (string.IsNullOrWhiteSpace(registerData.Username))
            {
                throw new ArgumentNullException(registerData.Username);
            }

            if (string.IsNullOrWhiteSpace(registerData.Password))
            {
                throw new ArgumentNullException(registerData.Password);
            }

            if (string.IsNullOrWhiteSpace(registerData.FullName))
            {
                throw new ArgumentNullException(registerData.FullName);
            }

            if (string.IsNullOrWhiteSpace(registerData.Email))
            {
                throw new ArgumentNullException(registerData.Email);
            }

            var user = await userService.CreateAsync(mapper.Map<RegisterRequest, User>(registerData));

            await passwordService.CreateAsync(new Password
            {
                UserId = user.Id,
                Hash = passwordService.Hash(registerData.Password),
            });

            await userEmailService.CreateAsync(new UserEmail {
                UserId = user.Id,
                Email = registerData.Email,
                IsVerified = false,
            });
        }
    }
}
