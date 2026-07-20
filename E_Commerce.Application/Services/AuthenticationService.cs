using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Services
{
    internal class AuthenticationService : IAuthenticationService
    {
        private readonly IIdentityService identityService;
        private readonly ITokenService tokenService;

        public AuthenticationService(IIdentityService identityService, ITokenService tokenService)
        {
            this.identityService = identityService;
            this.tokenService = tokenService;
        }

        public async Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default)
       => await identityService.CheckEmailExistAsync(email, ct);

        public async Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default)
        {
            var userResult = await identityService.FindByEmailAsync(email, ct);
            var user = userResult.Data;
            var roles = await identityService.GetRolesAsync(email, ct);

            var token = tokenService.CreateToken(user.Id, email, user.UserName!, roles.Data!);
            return new UserDTO() { DisplayName = user.DisplayName, Email = user.Email!, Token = token };

        }

        public async Task<Result<AddressDTO>> GetCurrenUserAddressAsync(string email, CancellationToken ct = default)
          => await identityService.GetUserAddressAsync(email, ct);


        public async Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken ct = default!)
        {
            var user = await identityService.FindByEmailAsync(loginDTO.Email, ct);
            if (!user.IsSuccess)
                return Result<UserDTO>.Fail(user.Errors);
            var passwordCheck = await identityService.CheckPasswordAsync(loginDTO.Email, loginDTO.Password, ct);

            if (!passwordCheck.IsSuccess)
                return Result<UserDTO>.Fail(user.Errors);
            if (!passwordCheck.Data)
                return Result<UserDTO>.Fail(Error.Unauthorized("User.Unauthorized", "Invalid in Password Or Email"));
            var rolesResult = await identityService.GetRolesAsync(loginDTO.Email, ct);
            var roles = rolesResult.Data;
            var token = tokenService.CreateToken(user.Data!.Id, user.Data.Email!, user.Data.UserName!, roles!);
            return new UserDTO
            {
                Email = loginDTO.Email,
                DisplayName = user.Data!.DisplayName,
                Token = token
            };


        }

        public async Task<Result<UserDTO>> RegisterAsync(RegistrationDTO registerDto, CancellationToken ct = default)
        {
            var result = await identityService.CreateUserAsync(registerDto, ct);
            if (!result.IsSuccess)
                return Result<UserDTO>.Fail(result.Errors);
            var user = result.Data;
            var rolesResult = await identityService.GetRolesAsync(user!.Email!, ct);
            var roles = rolesResult.Data;
            var token = tokenService.CreateToken(user!.Id, user.Email!, user.UserName!, roles!);

            return Result<UserDTO>.Ok(new UserDTO
            {
                Email = user.Email!,
                DisplayName = result.Data.DisplayName,
                Token = token
            });

        }

        public async Task<Result<AddressDTO>> UpsertUserAddressAsync(string email, AddressDTO address, CancellationToken ct = default)
        => await identityService.UpsertAddressAsync(email, address, ct);

    }
}
