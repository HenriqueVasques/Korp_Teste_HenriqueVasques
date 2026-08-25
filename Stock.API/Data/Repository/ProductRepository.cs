using Microsoft.EntityFrameworkCore;
using Stock.API.Data.Context;
using Stock.API.Interface.IRepository;
using Stock.API.Models;

namespace Stock.API.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(Product product)
        {
            await _appDbContext.Products.AddAsync(product);
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }

        public async Task Update(Product product)
        {
            _appDbContext.Products.Update(product);
            await Task.CompletedTask;
        }

        public async Task<bool> ExistProductCode(string productCode, int? currentProductId = null)
        {
            return await _appDbContext.Products
                .AnyAsync(p => p.ProductCode == productCode
                            && (!currentProductId.HasValue || p.Id != currentProductId.Value)
                            && !p.IsDeleted
                );
        }

        public async Task<bool> ExistDescription(string description, int? currentProductId = null)
        {
            return await _appDbContext.Products
                .AnyAsync(p => p.Description.ToLower() == description.ToLower()
                            && (!currentProductId.HasValue || p.Id != currentProductId.Value)
                            && !p.IsDeleted
                );
        }

        public async Task<Product?> GetById(int id)
        {
            return await _appDbContext.Products
                .Where(p => p.Id == id && !p.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            return await _appDbContext.Products
                .Where(p => !p.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetByProductCodes(List<string> productCodes)
        {
            return await _appDbContext.Products
                .Where(p => productCodes.Contains(p.ProductCode) && !p.IsDeleted)
                .ToListAsync();
        }
    }
}
