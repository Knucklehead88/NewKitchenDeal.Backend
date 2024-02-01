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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    public class CrudControllerBase<T> : BaseApiController where T: BaseEntity
    {
        private readonly ICrudService<T> _crudService;

        public CrudControllerBase(ICrudService<T> crudService)
        {
            _crudService = crudService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<T>>> Get()
        {
            var entities =  await _crudService.ListAllAsync();
            return Ok(entities);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<T>> Get(int id)
        {
            var entity = await _crudService.GetByIdAsync(id);
            if (entity == null)
                return NotFound();

            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post(T entity)
        {
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
