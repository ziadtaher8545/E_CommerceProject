using E_Commerce.Application.Common;
using E_Commerce.Application.DTOs.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Contracts
{
    public interface IAuthenticationService
    {
        Task<Result<UserDTO>> LoginAsync(LoginDTO loginDTO, CancellationToken ct = default);
        Task<Result<UserDTO>> RegisterAsync(RegistrationDTO registerDto, CancellationToken ct = default);
        Task<Result<bool>> CheckEmailExistAsync(string email, CancellationToken ct = default);
        Task<Result<UserDTO>> GetCurrentUserAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> GetCurrenUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDTO>> UpsertUserAddressAsync(string email, AddressDTO address, CancellationToken ct = default);

    }
}
