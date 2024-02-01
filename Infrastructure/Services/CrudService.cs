using Core.Entities;
using Core.Entities.Identity;
using Core.Interfaces;
using Infrastructure.Data.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class CrudService<T>: ICrudService<T> where T : BaseEntity
    {
        private readonly AppIdentityDbContext _context;

        public CrudService(AppIdentityDbContext context)
        {
            _context = context;
        }
        public async Task<IReadOnlyList<T>> ListAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
           await _context.Set<T>().AddAsync(entity);
           await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Attach(entity);
            _context.Set<T>().Entry(entity).State = EntityState.Modified;

            await _context.SaveChangesAsync();
        }

        public Task<bool> EntityExists(int id)
        {
            return _context.Set<T>().AnyAsync(e => e.Id == id);
        }


    }
}
