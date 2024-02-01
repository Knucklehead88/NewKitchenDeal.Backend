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
using Stripe;
using System.Collections.Generic;
using System.Drawing;

namespace API.Controllers
{

    public class BusinessInfoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICrudService<Trade> _tradeService;
        private readonly ICrudService<Language> _languageService;
        private readonly IMapper _mapper;


        public BusinessInfoController(UserManager<AppUser> userManager,
            ICrudService<Trade> tradeService,
            ICrudService<Language> languageService,
            IMapper mapper)
        {
            _mapper = mapper;
            _userManager = userManager;
            _tradeService = tradeService;
            _languageService = languageService;
        }

        [HttpPost]
        public async Task<ActionResult<ResponseBusinessInfoDto>> AddUserBusinessInfo(BusinessInfoDto businessInfoDto)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User);
            if(user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var languages = await _languageService.ListAllAsync();
            var mappedlanguages = languages.Where(l => businessInfoDto.SpokenLanguages.Any(sp => sp.Id == l.Id)).ToList();

            if(mappedlanguages.Count != businessInfoDto.SpokenLanguages.Count)
            {
                return BadRequest(new ApiResponse(400, "not all languages exist"));
            }

            var trades = await _tradeService.ListAllAsync();
            var mappedTrades = trades.Where(td => businessInfoDto.Trades.Any(t => td.Id == t.Id)).ToList();

            if (mappedTrades.Count != businessInfoDto.Trades.Count)
            {
                return BadRequest(new ApiResponse(400, "not all trades exist"));
            }

            var businessInfo = _mapper.Map<BusinessInfoDto, BusinessInfo>(businessInfoDto);
            businessInfo.SpokenLanguages.AddRange(mappedlanguages.Select(ml => new BusinessInfoLanguage()
            {
                Language = ml,
                BusinessInfo = businessInfo
            }));

            businessInfo.Trades.AddRange(mappedTrades.Select(t => new BusinessInfoTrade()
            {
                Trade = t,
                BusinessInfo = businessInfo
            }));
            user.BusinessInfo = businessInfo;
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var languageDtos = _mapper.Map<List<Language>, List<ResponseLanguageDto>>(mappedlanguages);
                var tradeDtos = _mapper.Map<List<Trade>, List<ResponseTradeDto>>(mappedTrades);
                var responseBusinessInfoDto = _mapper.Map<ResponseBusinessInfoDto>(user.BusinessInfo);
                responseBusinessInfoDto.SpokenLanguages = languageDtos;
                responseBusinessInfoDto.Trades = tradeDtos;

                return Ok(responseBusinessInfoDto);
            }
            return BadRequest(new ApiResponse(400, "Problem updating the user's business info"));

        }


        [HttpGet]
        public async Task<ActionResult<ResponseBusinessInfoDto>> GetUserBusinessInfo()
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User);

            if (user.BusinessInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var businessInfoDto = _mapper.Map<ResponseBusinessInfoDto>(user.BusinessInfo);

            var languages = await _languageService.ListAllAsync();
            var languagesDto = _mapper.Map<List<Language>, List<ResponseLanguageDto>>(languages.Where(l => user.BusinessInfo.SpokenLanguages.Any(sp => sp.LanguageId == l.Id)).ToList());

            var trades = await _tradeService.ListAllAsync();
            var tradesDto = _mapper.Map<List<Trade>, List<ResponseTradeDto>>(trades.Where(t => user.BusinessInfo.Trades.Any(td => td.TradeId == t.Id)).ToList());

            if (languagesDto.Count != 0) businessInfoDto.SpokenLanguages = languagesDto;
            if (tradesDto.Count != 0)
            {
                businessInfoDto.Trades = tradesDto;
            }

            return businessInfoDto;
        }


    }
}
