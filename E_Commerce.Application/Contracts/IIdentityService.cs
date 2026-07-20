using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IIdentityService
    {
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Result<bool>> CheckPasswordAsync(string email, string password, CancellationToken cancellationToken = default);
        Task<Result<IdentityUserResult>> CreateUserAsync(RegistrationDTO registerDto, CancellationToken ct = default);
        public Task<Result<IReadOnlyList<string>>> GetRolesAsync(string email, CancellationToken ct = default);
        public Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpsertAddressAsync(string email, AddressDTO address,CancellationToken ct = default);
    }

}

