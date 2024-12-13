using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    //public interface IProductRepository
    //{
    //    IQueryable<Product> GetAll();
    //    Task<Product> GetByIdAsync(Guid id);
    //    Task CreateAsync(Product product);
    //    Task UpdateAsync(Product product);
    //    Task DeleteAsync(Guid id);
    //}

    //public class ProductRepository : IProductRepository
    //{
    //    private readonly CrmContext _context;

    //    public ProductRepository(CrmContext context)
    //    {
    //        _context = context;
    //    }

    //    public IQueryable<Product> GetAll()
    //    {
    //        return _context.Products;
    //    }

    //    public async Task<Product> GetByIdAsync(Guid id)
    //    {
    //        return await _context.Products.FindAsync(id);
    //    }

    //    public async Task CreateAsync(Product product)
    //    {
    //        await _context.Products.AddAsync(product);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task UpdateAsync(Product product)
    //    {
    //        _context.Products.Update(product);
    //        await _context.SaveChangesAsync();
    //    }

    //    public async Task DeleteAsync(Guid id)
    //    {
    //        var product = await _context.Products.FindAsync(id);
    //        if (product != null)
    //        {
    //            _context.Products.Remove(product);
    //            await _context.SaveChangesAsync();
    //        }
    //    }
    //}
}
