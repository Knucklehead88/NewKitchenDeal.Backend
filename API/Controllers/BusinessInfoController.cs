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
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class BusinessInfoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;


        public BusinessInfoController(UserManager<AppUser> userManager,
            AppIdentityDbContext identityContext,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<ActionResult<BusinessInfoDto>> AddUserBusinessInfo(BusinessInfoDto businessInfoDto)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User);


            user.BusinessInfo = _mapper.Map<BusinessInfoDto, BusinessInfo>(businessInfoDto);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded) return Ok(_mapper.Map<BusinessInfoDto>(user.BusinessInfo));
            return BadRequest(new ApiResponse(400, "Problem updating the user's business info"));

        }


        [HttpGet]
        public async Task<ActionResult<BusinessInfoDto>> GetUserBusinessInfo()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User);

            if (user.BusinessInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return _mapper.Map<BusinessInfo, BusinessInfoDto>(user.BusinessInfo);

        }


    }
}
