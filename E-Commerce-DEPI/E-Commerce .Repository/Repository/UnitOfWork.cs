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
    public class UnitOfWork : IUnitOfWork
    {
        private readonly E_CommerceDbContext _context;

        public IProductRepository ProductRepository {  get; }
        public UnitOfWork(E_CommerceDbContext context, IProductRepository productRepository)
        {
            _context = context;
            ProductRepository = productRepository;
        }
        

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
