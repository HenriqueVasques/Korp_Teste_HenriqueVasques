using Stock.API.Models;

namespace Stock.API.Interface.IRepository
{
    public interface IProductRepository
    {
        Task Add(Product product);
        Task<bool> SaveChangesAsync();
        Task Update(Product product);
        Task<bool> ExistProductCode(string productCode, int? currentProductId = null);
        Task<bool> ExistDescription(string description, int? currentProductId = null);
        Task<Product?> GetById(int id);
        Task<IEnumerable<Product>> GetAll();
        Task<IEnumerable<Product>> GetByProductCodes(List<string> productCodes);
    }
}
