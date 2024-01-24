using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class PersonalInfoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public PersonalInfoController(UserManager<AppUser> userManager, 
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<PersonalInfoDto>> AddUserPerfonalInfo(PersonalInfoDto personalInfoDto)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User);

            user.PersonalInfo = _mapper.Map<PersonalInfoDto, PersonalInfo>(personalInfoDto);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return Ok(_mapper.Map<PersonalInfoDto>(user.PersonalInfo));

            return BadRequest(new ApiResponse(400, "Problem updating the user's personal info"));

        }


        [HttpGet]
        public async Task<ActionResult<PersonalInfoDto>> GetUserPerfonalInfo()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User);

            if (user.BusinessInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<PersonalInfo, PersonalInfoDto>(user.PersonalInfo);

        }


    }
}
