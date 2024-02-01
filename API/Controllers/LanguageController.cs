using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    public class LanguageController : CrudControllerBase<Language>
    {
        public LanguageController(ICrudService<Language> crudService) : base(crudService)
        {
        }
    }
}
