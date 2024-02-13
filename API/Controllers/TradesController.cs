using API.Dtos;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class TradesController : CrudControllerBase<Trade, ResponseTradeDto>
    {
        public TradesController(ICrudService<Trade> crudService, IMapper mapper) : base(crudService, mapper)
        {
        }
    }
}
