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
using System.Text;
using Google.Apis.Auth;
using Core.Entities;
using Microsoft.AspNetCore.WebUtilities;
using Core.Interfaces.Stripe;
using Stripe;
using Address = Core.Entities.Identity.Address;
using Infrastructure.Services;
using System.Net;
using API.Helpers;
using Core.Specifications;
using Infrastructure.Data;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ICustomersService _customerService;
        private readonly ICrudService<Trade> _tradeService;
        private readonly ICrudService<Location> _locationService;
        private readonly ICrudService<Language> _languageService;
        private readonly IMediaUploadService _mediaUploadService;
        private readonly MyAwsCredentials _credentials;
        private readonly IReadOnlyList<Language> _languageList;
        private readonly IReadOnlyList<Trade> _tradeList;
        private readonly IReadOnlyList<Location> _locationList;


        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ITokenService tokenService, 
            IMapper mapper, 
            IEmailService emailService, 
            ICustomersService customerService, 
            ICrudService<Trade> tradeService,
            ICrudService<Language> languageService,
            ICrudService<Location> locationService,
            IMediaUploadService mediaUploadService,
            MyAwsCredentials credentials)
        {
            _mapper = mapper;
            _emailService = emailService;
            _customerService = customerService;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _userManager = userManager;
            _tradeService = tradeService;
            _languageService = languageService;
            _locationService = locationService;
            _mediaUploadService = mediaUploadService;
            _credentials = credentials;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var userDto =  new UserBiDto
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Subscription = _mapper.Map<SubscriptionDto>(user.Subscription),
                CustomerId = user.CustomerId
            };

            if (user?.BusinessInfo != null)
            {
                var trades = await _tradeService.ListAllAsync();
                var tradesDto = _mapper.Map<List<Trade>, List<ResponseTradeDto>>(trades.Where(t => user.BusinessInfo.Trades.Any(td => td.TradeId == t.Id)).ToList());
                var businessInfoDto = _mapper.Map<ResponseBusinessInfoTradesDto>(user.BusinessInfo);

                if (tradesDto.Count != 0)
                {
                    businessInfoDto.Trades = tradesDto;
                }
                userDto.BusinessInfo = businessInfoDto;
            }

            return userDto;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailWithSubscription(loginDto.Email);

            if (user == null) return Unauthorized(new ApiResponse(401));

            //if (!user.EmailConfirmed)
            //{
            //    return new BadRequestObjectResult(new ApiValidationErrorResponse
            //    { Errors = [ "Email needs to be confirmed" ] });
            //}

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

            if (!result.Succeeded) return Unauthorized(new ApiResponse(401));

            return new UserDto
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Subscription = _mapper.Map<SubscriptionDto>(user.Subscription),
                CustomerId = user.CustomerId
            };
        }


        [Authorize]
        [HttpGet("getcontractors")]
        public async Task<ActionResult<Pagination<UserDto>>> GetUsers([FromQuery] UserSpecParams userParams)
        {
            var users = await _userManager.FindByUserParams(userParams);

            var userDtos = users.Select((user) => new UserDto
            {
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                Token = _tokenService.CreateToken(user),
                DisplayName = user.DisplayName,
                ProfilePictureUrl = user.ProfilePictureUrl,
                Subscription = _mapper.Map<SubscriptionDto>(user.Subscription),
                CustomerId = user.CustomerId
            }).ToList().AsReadOnly();
            return Ok(new Pagination<UserDto>(userParams.PageIndex, userParams.PageSize, userDtos.Count, userDtos));

        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse 
                    { Errors = ["Email address is in use"] });
            }
            var customer = await _customerService.CreateCustomerAsync(registerDto.DisplayName, registerDto.Email);
            if (customer == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid External Authentication."));
            }

            var user = new AppUser
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.DisplayName,
                CustomerId = customer.Id
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded) return BadRequest(new ApiResponse(400));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(registerDto.ClientURI, param);

            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", callback);
            //await _userManager.AddToRoleAsync(user, "Viewer");

            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                EmailConfirmed = user.EmailConfirmed,
                ProfilePictureUrl = user.ProfilePictureUrl,
                //Subscription = _mapper.Map<SubscriptionDto>(user.Subscription),
                CustomerId = user.CustomerId,
            };
        }

        [HttpGet("emailconfirmation")]
        public async Task<IActionResult> EmailConfirmation([FromQuery] string email, [FromQuery] string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var confirmResult = await _userManager.ConfirmEmailAsync(user, token);
            if (!confirmResult.Succeeded)
                return BadRequest(new ApiResponse(400, "Invalid Email Confirmation Request"));

            return Ok("Thank you for confirming your email");
        }

        [HttpGet("resendemailconfirmation")]
        public async Task<IActionResult> ResendEmailConfirmation([FromQuery]ResendEmailDto resendEmailDto)
        {
            var user = await _userManager.FindByEmailAsync(resendEmailDto.Email);
            if (user == null) return Unauthorized(new ApiResponse(401));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", user.Email }
            };

            var callback = QueryHelpers.AddQueryString(resendEmailDto.ClientURI, param);
            await _emailService.SendEmailAsync(user.Email, "Email Confirmation", callback);

            return Ok();
        }


        [HttpPost("externallogin")]
        public async Task<ActionResult<UserDto>> ExternalLogin([FromBody] ExternalAuthDto externalAuthDto)
        {
            var externalAuth = _mapper.Map<ExternalAuthDto, ExternalAuth>(externalAuthDto);
            var payload = await _tokenService.VerifyGoogleToken(externalAuth);
            if (payload == null)
                return BadRequest(new ApiResponse(400, "Invalid External Authentication."));
            var info = new UserLoginInfo(externalAuth.Provider, payload.Subject, externalAuth.Provider);
            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                user = await _userManager.FindByEmailWithSubscription(payload.Email);
                if (user == null)
                {
                    var customer = await _customerService.CreateCustomerAsync(user.DisplayName, user.Email);
                    if(customer == null)
                    {
                        return BadRequest(new ApiResponse(400, "We could not create a stripe customer."));
                    }
                    user = new AppUser { Email = payload.Email, UserName = payload.Email, DisplayName = payload.GivenName, CustomerId = customer.Id };
                    await _userManager.CreateAsync(user);
                    //prepare and send an email for the email confirmation
                    //await _userManager.AddToRoleAsync(user, "Viewer");
                    await _userManager.AddLoginAsync(user, info);
                }
                else
                {
                    await _userManager.AddLoginAsync(user, info);
                }
            }
            if (user == null)
                return BadRequest(new ApiResponse(400, "Invalid External Authentication."));
            //check for the Locked out account
            return new UserDto
            {
                DisplayName = user.DisplayName,
                Email = user.Email,
                Token = _tokenService.CreateToken(user),
                ProfilePictureUrl = user.ProfilePictureUrl,
                Subscription = _mapper.Map<SubscriptionDto>(user.Subscription),
                CustomerId = user.CustomerId
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

        //[Authorize]
        //[HttpPut("subscription")]
        //public async Task<ActionResult<AddressDto>> UpdateSubscription(Subscription subscription) 
        //{
        //    var user = await _userManager.FindUserByClaimsPrincipleWithAddress(User);

        //    //user.Address = _mapper.Map<AddressDto, Address>(address);

        //    var result = await _userManager.UpdateAsync(user);

        //    if (result.Succeeded) return Ok(_mapper.Map<AddressDto>(user.Address));

        //    return BadRequest("Problem updating the user");
        //}

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Request"));

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(401, "Email was not found."));


            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var param = new Dictionary<string, string>
            {
                {"token", token },
                {"email", forgotPasswordDto.Email }
            };

            var callback = QueryHelpers.AddQueryString(forgotPasswordDto.ClientURI, param); 
            await _emailService.SendEmailAsync(user.Email, "Reset password token", callback);
            
            return Ok();
        }

        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Request"));

            var user = await _userManager.FindByEmailAsync(changePasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(401, "Email was not found."));

            var changePassResult = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!changePassResult.Succeeded)
            {
                var errors = changePassResult.Errors.Select(e => e.Description);

                return new BadRequestObjectResult(new ApiValidationErrorResponse
                    { Errors = errors });
            }

            return Ok();
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Request"));

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return BadRequest(new ApiResponse(401, "Email was not found."));

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token, resetPasswordDto.Password);
            if (!resetPassResult.Succeeded)
            {
                var errors = resetPassResult.Errors.Select(e => e.Description);

                return new BadRequestObjectResult(new ApiValidationErrorResponse
                { Errors = errors });
            }

            return Ok();
        }

        [HttpPost("getintouch")]
        public async Task<IActionResult> GetInTouch([FromBody] SendToEmailDto sendToEmailDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, "Invalid Request"));

            var sendToEmail = _mapper.Map<SendToEmailDto, SendToEmail>(sendToEmailDto);

            await _emailService.GetInTouchAsync(sendToEmail);
            return Ok();
        }

        [HttpPost("profilepicture")]
        public async Task<ActionResult<string>> AddProfilePicture(IFormFile profilePicture)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User);

            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            if (!FileValidator.IsFileExtensionAllowed(profilePicture, [".jpeg", ".jpg", ".png", ".gif", ".bmp", ".tiff", ".svg"]))
                return BadRequest(new ApiResponse(400, "Invalid file type. Please upload a JPEG, JPG, PNG, GIF or BMP file."));

            if (!FileValidator.IsFileSizeWithinLimit(profilePicture, 5 * 1024 * 1024))
                return BadRequest(new ApiResponse(400, "File size exceeds the maximum allowed size (5 MB)."));

            var key = $"profile_picture/{Guid.NewGuid()}";
            var response = await _mediaUploadService.UploadFileAsync(key, profilePicture);

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return BadRequest(new ApiResponse(400, "Image could not be uploaded."));
            }

            var profilePictureUrl = $"{_credentials.S3Url}/{key}";
            user.ProfilePictureUrl = profilePictureUrl;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400, "Problem updating the user's profile picture."));
            }
            return Ok(profilePictureUrl);
        }

        [HttpDelete("profilepicture")]
        public async Task<IActionResult> DeleteUserPerfonalInfoProfilePicture()
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);

            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            if (string.IsNullOrEmpty(user.ProfilePictureUrl))
            {
                return NotFound(new ApiResponse(404, "The user does not have a profile picture"));
            }

            var response = await _mediaUploadService.DeleteFileAsync(user.ProfilePictureUrl);
            return response.HttpStatusCode switch
            {
                HttpStatusCode.NoContent => Ok(),
                HttpStatusCode.NotFound => NotFound(new ApiResponse(404, "The user does not have a profile picture")),
                _ => BadRequest(new ApiResponse(400))
            };
        }

    }
}