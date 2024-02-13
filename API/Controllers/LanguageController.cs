using API.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class LanguageController : CrudControllerBase<Language, ResponseLanguageDto>
    {
        public LanguageController(ICrudService<Language> crudService, IMapper mapper) : base(crudService, mapper)
        {
        }
    }
}
