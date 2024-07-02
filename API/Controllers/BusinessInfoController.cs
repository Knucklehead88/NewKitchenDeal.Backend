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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Net.Mail;

namespace API.Controllers
{

    public class BusinessInfoController : BaseApiController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICrudService<Trade> _tradeService;
        private readonly ICrudService<Language> _languageService;
        private readonly IMediaUploadService _mediaUploadService;
        private readonly IMapper _mapper;
        private readonly MyAwsCredentials _credentials;

        public BusinessInfoController(UserManager<AppUser> userManager,
            ICrudService<Trade> tradeService,
            ICrudService<Language> languageService,
            IMediaUploadService mediaUploadService,
            IMapper mapper,
            MyAwsCredentials credentials)
        {
            _mapper = mapper;
            _credentials = credentials;
            _userManager = userManager;
            _tradeService = tradeService;
            _languageService = languageService;
            _mediaUploadService = mediaUploadService;
        }

        [HttpPost("videopresentation")]
        public async Task<IActionResult> AddVideoPresentation(IFormFile videoPresentationFile, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailFromClaimsPrincipal(User);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            if (user.Subscription == null)
            {
                return BadRequest(new ApiResponse(400, "User doesn't have a subscription."));
            }

            if (user.Subscription.Status.ToLower().Equals("active"))
            {
                return BadRequest(new ApiResponse(400, "User doesn't have an active subscription."));
            }

            if (user.Subscription.PlanType.ToLower().Equals("basic"))
            {
                return BadRequest(new ApiResponse(400, "User's plan doesn't allow him to upload presentation videos."));
            }

            if (videoPresentationFile == null)
            {
                return BadRequest(new ApiResponse(400));
            }

            if (!FileValidator.IsFileExtensionAllowed(videoPresentationFile, [".mp4",".m4p", ".m4v", ".mpg", ".mp2", ".mpeg",
                ".mpe", ".mpv",".wmv", ".avi", ".gif", ".flv", ".mkv", ".mov"]))
                return BadRequest(new ApiResponse(400, "Invalid file type. Please upload a MP4, MOV, AVI, GIF, WMV, FLV or MPEG file."));

            int fileSize = user.Subscription.PlanType.ToLower().Equals("professional") ?
                        user.Subscription.PlanType.ToLower().Equals("professional plus") ? 50 : 30 : 30;

            if (!FileValidator.IsFileSizeWithinLimit(videoPresentationFile, fileSize * 1024 * 1024))
                return BadRequest(new ApiResponse(400, $"File size exceeds the maximum allowed size ({fileSize} MB)."));

            var key = $"video_presentation/{Guid.NewGuid()}";
            var response = await _mediaUploadService.UploadFileAsync(key, videoPresentationFile, cancellationToken);
            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                return BadRequest(new ApiResponse(400, "Video could not be uploaded."));
            }

            return Ok($"{_credentials.S3Url}/{key}");
        }

        [HttpPost]
        public async Task<ActionResult<ResponseBusinessInfoDto>> AddUserBusinessInfo(BusinessInfoDto businessInfoDto, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User, cancellationToken);
            if(user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var originalBusinessInfoTrades = user?.BusinessInfo?.Trades;

            var languages = await _languageService.ListAllAsync();
            var mappedlanguages = languages.Where(l => businessInfoDto.SpokenLanguages.Any(sp => sp.Id == l.Id)).ToList();
            if(mappedlanguages.Count != businessInfoDto.SpokenLanguages.Count)
            {
                return BadRequest(new ApiResponse(400, "not all languages exist"));
            }

            var trades = await _tradeService.ListAllAsync();
            var mappedTrades = trades?.Where(td => businessInfoDto.Trades.Any(t => td.Id == t.Id));

            if (mappedTrades != null &&
                businessInfoDto.Trades != null &&
                mappedTrades.Count() != businessInfoDto.Trades.Count)
            {
                return BadRequest(new ApiResponse(400, "not all trades exist"));
            }

            var businessInfo = _mapper.Map<BusinessInfoDto, BusinessInfo>(businessInfoDto);
            businessInfo.SpokenLanguages.AddRange(mappedlanguages.Select(ml => new BusinessInfoLanguage()
            {
                Language = ml,
                BusinessInfo = businessInfo
            }));

            if(businessInfoDto.Trades != null)
            {
                businessInfo.Trades.AddRange(mappedTrades.Select(t => new BusinessInfoTrade()
                {
                    Trade = t,
                    BusinessInfo = businessInfo
                }));
            }
            else if (originalBusinessInfoTrades != null && originalBusinessInfoTrades?.Count > 0)
            {
                businessInfo.Trades.AddRange(originalBusinessInfoTrades);
            }

            var businessInfoLocations = businessInfoDto.Locations.Select(l => {
                var newBusinessInfo = new BusinessInfoLocation()
                {
                    Location = _mapper.Map<Location>(l),
                    BusinessInfo = businessInfo
                };
                return newBusinessInfo;
            }).ToList();

            businessInfo.Locations = businessInfoLocations;

            user.BusinessInfo = businessInfo;
            
            cancellationToken.ThrowIfCancellationRequested();

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                var languageDtos = _mapper.Map<List<Language>, List<ResponseLanguageDto>>(mappedlanguages);
                var tradeDtos = user.BusinessInfo?.Trades != null ? _mapper.Map<List<Trade>, List<ResponseTradeDto>>(user.BusinessInfo?.Trades?.Select(t => t.Trade)?.ToList()) : [];
                var responseBusinessInfoDto = _mapper.Map<ResponseBusinessInfoDto>(user.BusinessInfo);
                responseBusinessInfoDto.SpokenLanguages = languageDtos;
                responseBusinessInfoDto.Trades = tradeDtos;
                responseBusinessInfoDto.Locations = businessInfoDto.Locations;

                return Ok(responseBusinessInfoDto);
            }
            return BadRequest(new ApiResponse(400, "Problem updating the user's business info"));

        }

        [HttpPost("addbusinessinfotrades")]
        public async Task<ActionResult<List<ResponseTradeDto>>> AddBusinessInfoTrades(TradeDto[] tradeDtos)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User, default);
            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var trades = await _tradeService.ListAllAsync();
            var mappedTrades = trades.Where(td => tradeDtos.Any(t => td.Id == t.Id)).ToList();

            if (mappedTrades.Count != tradeDtos.Length)
            {
                return BadRequest(new ApiResponse(400, "not all trades exist"));
            }

            var businessInfo = new BusinessInfo();
            businessInfo ??= user?.BusinessInfo;

            businessInfo.Trades.AddRange(mappedTrades.Select(t => new BusinessInfoTrade()
            {
                Trade = t,
                BusinessInfo = businessInfo
            }));
            user.BusinessInfo = businessInfo;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                return _mapper.Map<List<Trade>, List<ResponseTradeDto>>(mappedTrades);
            }
            return BadRequest(new ApiResponse(400, "Problem updating the user's business info trades"));
        }


        [HttpGet]
        public async Task<ActionResult<ResponseBusinessInfoDto>> GetUserBusinessInfo(CancellationToken cancellationToken)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User, cancellationToken);

            if (user?.BusinessInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }

            var businessInfoDto = _mapper.Map<ResponseBusinessInfoDto>(user.BusinessInfo);

            cancellationToken.ThrowIfCancellationRequested();

            var languages = await _languageService.ListAllAsync();
            var languagesDto = _mapper.Map<List<Language>, List<ResponseLanguageDto>>(languages.Where(l => user.BusinessInfo.SpokenLanguages.Any(sp => sp.LanguageId == l.Id)).ToList());

            var trades = await _tradeService.ListAllAsync();
            var tradesDto = _mapper.Map<List<Trade>, List<ResponseTradeDto>>(trades.Where(t => user.BusinessInfo.Trades.Any(td => td.TradeId == t.Id)).ToList());

            var locationsDto = _mapper.Map<List<Location>, List<LocationDto>>(user.BusinessInfo.Locations.Select(l => l.Location).ToList());
            if (languagesDto.Count != 0)
            {
                businessInfoDto.SpokenLanguages = languagesDto;
            }

            if (tradesDto.Count != 0)
            {
                businessInfoDto.Trades = tradesDto;
            }

            if (locationsDto.Count != 0)
            {
                businessInfoDto.Locations = locationsDto;
            }

            return businessInfoDto;
        }

        [HttpDelete("videopresentation")]
        public async Task<IActionResult> DeleteUserBusinessInfoVideoPresentation(CancellationToken cancellationToken)
        {
            var user = await _userManager.FindUserByClaimsPrincipleWithBusinessInfo(User, cancellationToken);

            if (user == null)
            {
                return NotFound(new ApiResponse(404));
            }

            if (user.BusinessInfo == null)
            {
                return NotFound(new ApiResponse(404));
            }

            if (string.IsNullOrEmpty(user.BusinessInfo.VideoPresentation))
            {
                return NotFound(new ApiResponse(404));
            }

            var response = await _mediaUploadService.DeleteFileAsync(user.BusinessInfo.VideoPresentation, cancellationToken);
            return response.HttpStatusCode switch
            {
                HttpStatusCode.NoContent => Ok(),
                HttpStatusCode.NotFound => NotFound(new ApiResponse(404)),
                _ => BadRequest(new ApiResponse(400))
            };
        }
    }
}
