using API.Dtos;
using API.Errors;
using AutoMapper;
using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructue.Data;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;
using static StackExchange.Redis.Role;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class CrudControllerBase<T, TResult> : BaseApiController where T: BaseEntity
    {
        private readonly ICrudService<T> _crudService;
        private readonly IMapper _mapper;


        public CrudControllerBase(ICrudService<T> crudService, IMapper mapper)
        {
            _crudService = crudService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<TResult>>> Get()
        {
            var entities =  await _crudService.ListAllAsync();

            return Ok(_mapper.Map<IReadOnlyList<TResult>>(entities));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(int id)
        {
            var entity = await _crudService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(_mapper.Map<TResult>(entity));
        }

        [HttpPost]
        public async Task<IActionResult> Post(TResult entityDto)
        {
            var entity = _mapper.Map<T>(entityDto);

            await _crudService.AddAsync(entity);
            return CreatedAtAction("Post", new { id = entity.Id }, entity);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, T entity)
        {
            if (id != entity.Id)
                return BadRequest();

            if (!await _crudService.EntityExists(id))
                return NotFound();
            
            await _crudService.UpdateAsync(entity);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _crudService.GetByIdAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            await _crudService.DeleteAsync(entity);

            return NoContent();
        }
    }
}
