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
using Microsoft.Extensions.Options;
using System.IO;
using System.Net;
using System.Threading;

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
        public async Task<ActionResult<ResponsePersonalInfoDto>> AddUserPerfonalInfo(PersonalInfoDto personalInfoDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User, cancellationToken);
            
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            user.PersonalInfo = _mapper.Map<PersonalInfoDto, PersonalInfo>(personalInfoDto);

            if (personalInfoDto?.Locations != null)
            {
                var personalInfoLocations = personalInfoDto.Locations.Select(l => new PersonalInfoLocation()
                {
                    Location = _mapper.Map<Location>(l),
                    PersonalInfo = user.PersonalInfo
                }).ToList();
                user.PersonalInfo.Locations = personalInfoLocations;
            }

            cancellationToken.ThrowIfCancellationRequested();
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
        public async Task<ActionResult<ResponsePersonalInfoDto>> GetUserPerfonalInfo(CancellationToken cancellationToken)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithPersonalInfo(User, cancellationToken);

            if (user?.PersonalInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }
            var personalInfoDto = _mapper.Map<PersonalInfo, ResponsePersonalInfoDto>(user.PersonalInfo);

            var locationsDto = _mapper.Map<List<Location>, List<LocationDto>>(user.PersonalInfo.Locations.Select(l => l.Location).ToList());

            if (locationsDto.Count != 0)
            {
                personalInfoDto.Locations = locationsDto;
            }

            return personalInfoDto;

        }
    }
}
