using AutoMapper;
using Stock.API.DTOs.Product;
using Stock.API.Interface.IRepository;
using Stock.API.Interface.IService;
using Stock.API.Models;

namespace Stock.API.Service
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        public async Task<ProductResponseDto> CreateProduct(ProductCreateDto dto)
        {
            var existProductCode = await _productRepository.ExistProductCode(dto.ProductCode);
            if (existProductCode)
            {
                throw new InvalidOperationException("Código de Produto já existente, atualize o produto para abastecer o estoque.");
            }

            var existDescription = await _productRepository.ExistDescription(dto.Description);
            if (existDescription)
            {
                throw new InvalidOperationException("Descrição de Produto já existente, atualize o produto para abastecer o estoque.");
            }

            var product = _mapper.Map<Product>(dto);

            await _productRepository.Add(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductResponseDto>(product);
        }
    

        public async Task<ProductResponseDto> UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            if (await _productRepository.ExistProductCode(dto.ProductCode, id))
                throw new InvalidOperationException("Já existe outro produto cadastrado com este código.");

            if (await _productRepository.ExistDescription(dto.Description, id))
                throw new InvalidOperationException("Já existe outro produto cadastrado com esta descrição.");

            _mapper.Map(dto, product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task DeleteProduct(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");
            product.IsDeleted = true;
            await _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task DeductStock(IEnumerable<DeductStockItemDto> items)
        {
            if (items == null || !items.Any())
                throw new InvalidOperationException("A lista de itens para baixa no estoque não pode estar vazia.");

            foreach (var item in items)
            {
                if (item.Quantity <= 0)
                    throw new InvalidOperationException($"A quantidade do produto '{item.ProductCode}' deve ser maior que zero.");
            }

            var productCodes = items.Select(i => i.ProductCode).Distinct().ToList();
            var products = await _productRepository.GetByProductCodes(productCodes);

            foreach (var item in items)
            {
                var product = products.FirstOrDefault(p => p.ProductCode == item.ProductCode);

                if (product == null)
                    throw new KeyNotFoundException($"Produto com código '{item.ProductCode}' não foi encontrado no estoque.");

                if (product.Balance < item.Quantity)
                    throw new InvalidOperationException($"Saldo insuficiente para o produto '{product.Description}' (Código: {product.ProductCode}). Disponível: {product.Balance}, Solicitado: {item.Quantity}.");
            }

            foreach (var item in items)
            {
                var product = products.First(p => p.ProductCode == item.ProductCode);
                product.Balance -= item.Quantity;
            }

            await _productRepository.SaveChangesAsync();
        }

        public async Task<ProductResponseDto> GetById(int id)
        {
            var product = await _productRepository.GetById(id);
            if (product == null)
                throw new KeyNotFoundException("Produto não encontrado.");

            return _mapper.Map<ProductResponseDto>(product);
        }

        public async Task<IEnumerable<ProductResponseDto>> GetAll()
        {
            var products = await _productRepository.GetAll();
            return _mapper.Map<IEnumerable<ProductResponseDto>>(products);
        }
    }
}
