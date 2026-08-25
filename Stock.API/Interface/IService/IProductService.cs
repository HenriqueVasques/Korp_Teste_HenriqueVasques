using Stock.API.DTOs.Product;

namespace Stock.API.Interface.IService
{
    public interface IProductService
    {
        Task<ProductResponseDto> CreateProduct(ProductCreateDto dto);
        Task<ProductResponseDto> UpdateProduct(int id, ProductUpdateDto dto);
        Task DeleteProduct(int id);
        Task<ProductResponseDto> GetById(int id);
        Task<IEnumerable<ProductResponseDto>> GetAll();
        Task DeductStock(IEnumerable<DeductStockItemDto> items);
    }
}
