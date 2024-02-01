using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TradesController : CrudControllerBase<Trade>
    {
        public TradesController(ICrudService<Trade> crudService) : base(crudService)
        {
        }
    }
}
