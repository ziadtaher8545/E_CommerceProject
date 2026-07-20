using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using E_Commerce.Infrastructure.Identity.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Infrastructure.Identity.Services
{
    internal class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public IdentityService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default)
        => await userManager.Users.CountAsync(u => u.Email == email) > 0;

        public async Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<bool>.Fail(Error.NotFound("User Not Found", $"User with Email {email} is Not Found "));

            return await userManager.CheckPasswordAsync(user, password);
        }

        public async Task<Result<IdentityUserResult>> CreateUserAsync(RegistrationDTO registerDto, CancellationToken cancellationToken = default)
        {
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                DisplayName = registerDto.DisplayName,
                UserName = registerDto.UserName

            };
            var reslut = await userManager.CreateAsync(user, registerDto.Password);
            if (!reslut.Succeeded)
            {
                var errors = reslut.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);

            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
        }

        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user == null)
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Not Found", $"User with Email {email} is Not Found "));
            return new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName);
        }

        public async Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null) return Result<IReadOnlyList<string>>.Fail(Error.NotFound($"User '{email}' not found"));
            var roles = await userManager.GetRolesAsync(user);
            return roles.ToList();
        }

        public async Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct)
        {
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
            if (user.Address == null)
                return Error.NotFound("Addres.NotFound", $"The Addres of User that has email :{email} is not Found");
            return new AddressDTO()
            {
                City = user.Address.City,
                LastName = user.Address.LastName,
                FirstName = user.Address.FirstName,
                Country = user.Address.Country,
                Street = user.Address.Street,
            };
        }

        public async Task<Result<AddressDTO>> UpsertAddressAsync(string email, AddressDTO address, CancellationToken ct = default)
        {
            var user = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email);
            if (user.Address == null)
            {
                user.Address = new Address()
                {
                    FirstName = address.FirstName,
                    LastName = address.LastName,
                    Street = address.Street,
                    City = address.City,
                    Country = address.Country
                };
            }
            else
            {
                user.Address.FirstName = address.FirstName;
                user.Address.LastName = address.LastName;
                user.Address.Street = address.Street;
                user.Address.City = address.City;
                user.Address.Country = address.Country;

            }
            var result = await userManager.UpdateAsync(user);
            if (result.Succeeded)
                return address;
            return Error.Failure("Failure.updateOrInsertFailed", $"Failure to Update or Insert the address with email{email}");
        }
    }
}
