using API.Dtos;
using API.Errors;
using API.Extensions;
using AutoMapper;
using Core.Entities;
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
        private readonly ICrudService<Location> _locationService;

        public PersonalInfoController(UserManager<AppUser> userManager, 
            IMapper mapper, ICrudService<Location> locationService)
        {
            _mapper = mapper;
            _locationService = locationService;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<PersonalInfoDto>> AddUserPerfonalInfo(PersonalInfoDto personalInfoDto)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var personalInfoLocations = personalInfoDto.Locations.Select(l => new PersonalInfoLocation()
            {
                Location = _mapper.Map<Location>(l),
                PersonalInfo = user.PersonalInfo
            }).ToList();

            user.PersonalInfo = _mapper.Map<PersonalInfoDto, PersonalInfo>(personalInfoDto);
            user.PersonalInfo.Locations = personalInfoLocations;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                var responsePersonalInfoDto = _mapper.Map<PersonalInfoDto>(user.PersonalInfo);
                responsePersonalInfoDto.Locations = personalInfoDto.Locations;
                return Ok(responsePersonalInfoDto);
            }

            return BadRequest(new ApiResponse(400, "Problem updating the user's personal info"));

        }


        [HttpGet]
        public async Task<ActionResult<PersonalInfoDto>> GetUserPerfonalInfo()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User);

            if (user.PersonalInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var personalInfoDto = _mapper.Map<PersonalInfo, PersonalInfoDto>(user.PersonalInfo);

            var locations = await _locationService.ListAllAsync();
            var locationsDto = _mapper.Map<List<Location>, List<LocationDto>>(locations.Where(l => user.PersonalInfo.Locations.Any(lo => lo.LocationId == l.Id)).ToList());
            
            if (locationsDto.Count != 0)
            {
                personalInfoDto.Locations = locationsDto;
            }

            return personalInfoDto;

        }


    }
}
