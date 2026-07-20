using E_Commerce.Application.Common;
using E_Commerce.Application.Contracts;
using E_Commerce.Application.DTOs.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceProject.Controllers
{

    public class AuthentactionController : BaseController
    {
        private readonly IAuthenticationService authenticationService;

        public AuthentactionController(IAuthenticationService authenticationService)
        {
            this.authenticationService = authenticationService;
        }
        [HttpPost("Login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO) => ToActionResult(await authenticationService.LoginAsync(loginDTO));
        [HttpPost("Register")]
        public async Task<ActionResult<UserDTO>> Register(RegistrationDTO registrationDTO) => ToActionResult(await authenticationService.RegisterAsync(registrationDTO));
        [HttpGet("EmailExist")]
        public async Task<ActionResult<bool>> CheckEmailExist([FromQuery] string email)
            => ToActionResult(await authenticationService.CheckEmailExistAsync(email));


        [Authorize]
        [HttpGet("AuthMe")]
        public async Task<ActionResult<UserDTO>> AuthMe()
        {
            var email = GetEmailByToken();
            return ToActionResult(await authenticationService.GetCurrentUserAsync(email));
        }
        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDTO>> GetUserAddress()
        {
            var email = GetEmailByToken();
            return ToActionResult(await authenticationService.GetCurrenUserAddressAsync(email));
        }
        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDTO>> UpsertUserAddress(AddressDTO address)
        {
            var email = GetEmailByToken();
            return ToActionResult(await authenticationService.UpsertUserAddressAsync(email, address));
        }


    }
}
