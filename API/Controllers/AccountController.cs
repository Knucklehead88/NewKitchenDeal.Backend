using System.Security.Claims;
using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, IMapper mapper)
        {
            _mapper = mapper;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            };
        }

        [Authorize]
        [HttpGet("getallusers")]
        public ActionResult<IList<UserDto>> GetAllUsers()
        {
            return _userManager.Users.Select((user) => new UserDto
            {
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName
            }).ToList();
        }

        //[HttpPost("LoginWithGoogle")]

        //public IActionResult LoginWithGoogle(string returnUrl = "/")
        //{
        //    var props = new AuthenticationProperties
        //    {
        //        RedirectUri = Url.Action("GoogleLoginCallback"),
        //        Items =
        //    {
        //        { "returnUrl", returnUrl }
        //    }
        //    };
        //    return Challenge(props, GoogleDefaults.AuthenticationScheme);
        //}

        //[HttpPost("GoogleLoginCallback")]
        //public async Task<ActionResult<UserDto>> GoogleLoginCallback()
        //{
        //    // read google identity from google's cookie
        //    var result = await HttpContext.AuthenticateAsync(
        //        GoogleDefaults.AuthenticationScheme);

        //    if (result.Principal == null)
        //        throw new Exception("Could not create a principal");
        //    var externalClaims = result.Principal.Claims.ToList();

        //    var subjectIdClaim = externalClaims.FirstOrDefault(
        //        x => x.Type == ClaimTypes.NameIdentifier);

        //    if (subjectIdClaim == null)
        //        return Unauthorized(new ApiResponse(401));

        //    var subjectValue = subjectIdClaim.Value;

        //    var user = await _userManager.FindByNameAsync(subjectValue);

        //    if (user == null) return Unauthorized(new ApiResponse(401));

        //    return new UserDto
        //    {
        //        Email = user.Email,
        //        Token = _tokenService.CreateToken(user),
        //        DisplayName = user.DisplayName
        //    };
        //}

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse 
                    { Errors = new[] { "Email address is in use" } });
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Token = _tokenService.CreateToken(user),
                Email = user.Email
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }

        [Authorize]
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

            return _mapper.Map<Address, AddressDto>(user.Address);
        }

        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress(AddressDto address)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

            user.Address = _mapper.Map<AddressDto, Address>(address);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

            return BadRequest("Problem updating the user");
        }
    }
}