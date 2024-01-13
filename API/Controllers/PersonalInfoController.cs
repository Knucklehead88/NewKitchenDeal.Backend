using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace API.Controllers
{

    public class PersonalInfoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppIdentityDbContext _appIdentityDbContext;
        private readonly IMapper _mapper;


        public PersonalInfoController(UserManager<AppUser> userManager, 
            AppIdentityDbContext appIdentityDbContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _appIdentityDbContext = appIdentityDbContext;
        }

        [HttpPost]
        public async Task<ActionResult<PersonalInfo>> AddUserPerfonalInfo(PersonalInfoDto personalInfoDto)
        {
            var user = await _userManager.FindByEmailAsync(personalInfoDto.AppUserEmail);

            if (user == null) return NotFound(new ApiResponse(404));

            var personalInfo = _mapper.Map<PersonalInfoDto, PersonalInfo>(personalInfoDto);
            user.PersonalInfo = personalInfo;

            await _appIdentityDbContext.Users.AddAsync(user);
            _appIdentityDbContext.SaveChanges();
            
            return Ok(personalInfo);

        }


        [HttpGet]
        public async Task<ActionResult<PersonalInfoDto>> GetUserPerfonalInfo(string appUserEmail)
        {
            var user = await _userManager.FindByIdAsync(appUserEmail);

            if (user == null) return NotFound(new ApiResponse(404));

            return _mapper.Map<PersonalInfoDto>(user.PersonalInfo);

        }


    }
}
