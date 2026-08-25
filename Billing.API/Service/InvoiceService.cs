using AutoMapper;
using Billing.API.Data.Repository;
using Billing.API.DTOs.DeductStockItem;
using Billing.API.DTOs.Invoices;
using Billing.API.Interface.IRepository;
using Billing.API.Interface.IService;
using Billing.API.Models;
using System.Net.Http;
using static Billing.API.Enum.InvoiceStatusEnum;


namespace Billing.API.Service
{
    public class InvoiceService : IInvoiceService
    {
        readonly IInvoiceRepository _iInvoiceRepository;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        public InvoiceService(IInvoiceRepository iInvoiceRepository, IMapper mapper, HttpClient httpClient)
        {
            _iInvoiceRepository = iInvoiceRepository;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<InvoiceResponseDto> CreateInvoice(InvoiceCreateDto dto)
        {
            var invoice = _mapper.Map<Invoice>(dto);

            invoice.Number = await GetNextNumberAsync();
            invoice.IssueDate = DateTime.UtcNow;
            invoice.Status = InvoiceStatus.Open;

            await _iInvoiceRepository.Add(invoice);


            return _mapper.Map<InvoiceResponseDto>(invoice);
        }

        public async Task DeleteInvoice(int id)
        {
            if(id <= 0)
                throw new ArgumentException("O ID da nota fiscal deve ser maior que zero.");

            var invoice = await _iInvoiceRepository.GetById(id);
            
            if(invoice == null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            if(invoice.Status != InvoiceStatus.Open)
                throw new InvalidOperationException("Não é possível deletar uma nota fiscal que não esteja aberta.");

            invoice.IsDeleted = true;
            foreach (var item in invoice.Items)
            {
                item.IsDeleted = true;
            }

            await _iInvoiceRepository.Update(invoice);
            await _iInvoiceRepository.SaveChangesAsync();
        }
        public async Task<InvoiceResponseDto> CloseInvoice(int id)
        {
            var invoice = await _iInvoiceRepository.GetById(id);
            if (invoice == null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            if (invoice.Status == InvoiceStatus.Closed)
                throw new InvalidOperationException("Esta nota fiscal já está fechada.");

            var stockItems = invoice.Items.Select(item => new DeductStockItemDto
            {
                ProductCode = item.ProductCode,
                Quantity = item.Quantity
            });

            var response = await _httpClient.PostAsJsonAsync("api/product/deduct-stock", stockItems);

            if (!response.IsSuccessStatusCode)
            {
                var errorResponse = await response.Content.ReadFromJsonAsync<StockErrorResponse>();
                throw new InvalidOperationException(errorResponse?.Error ?? "Falha ao dar baixa no estoque no Stock.API.");
            }

            invoice.Status = InvoiceStatus.Closed;

            await _iInvoiceRepository.Update(invoice);
            await _iInvoiceRepository.SaveChangesAsync();

            return _mapper.Map<InvoiceResponseDto>(invoice);
        }


        public async Task<InvoiceResponseDto?> GetById(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID da nota fiscal deve ser maior que zero.");

            var invoice = await _iInvoiceRepository.GetById(id);
            if (invoice == null)
                throw new KeyNotFoundException("Nota fiscal não encontrada.");

            return _mapper.Map<InvoiceResponseDto>(invoice);
        }

        public async Task<IEnumerable<InvoiceResponseDto>> GetAll()
        {
            var invoices = await _iInvoiceRepository.GetAll();
            return _mapper.Map<IEnumerable<InvoiceResponseDto>>(invoices);
        }

        private async Task<int> GetNextNumberAsync()
        {
            var maxNumber = await _iInvoiceRepository.GetMaxNumberAsync();
            return maxNumber + 1;
        }
    }
}   
