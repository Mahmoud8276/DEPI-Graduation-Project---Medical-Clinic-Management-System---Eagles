using E_Commerce.Data.Context;
using E_Commerce_.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce_.Repository.Repository
{
    public class GenaricRepository<T> : IGenaricRepository<T> where T : class
    {
        private readonly E_CommerceDbContext _context;

        public GenaricRepository(E_CommerceDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
         =>  await _context.Set<T>() .AddAsync(entity);


        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }


        public async Task<T?> GetByIdAsync(int id)
         => await _context.Set<T>().FindAsync(id);


        public void Update(T entity)
        {
           _context.Set<T>() .Update(entity);
        }


       public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }
    }
}
