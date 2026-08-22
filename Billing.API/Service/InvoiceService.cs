using Billing.API.Interface.IRepository;
using Billing.API.DTOs.Invoices;
using Billing.API.Interface.IService;
using Billing.API.Models;
using AutoMapper;
using static Billing.API.Enum.InvoiceStatusEnum;


namespace Billing.API.Service
{
    public class InvoiceService : IInvoiceService
    {
        readonly IInvoiceRepository _iInvoiceRepository;
        private readonly IMapper _mapper;
        public InvoiceService(IInvoiceRepository iInvoiceRepository, IMapper mapper) 
        { 
            _iInvoiceRepository = iInvoiceRepository;
            _mapper = mapper;
        }

        public async Task<InvoiceResponseDto> CreateInvoice()
        {
            var nextNumber = await GetNextNumberAsync();

            var invoice = new Invoice
            {
                Number = nextNumber
            };

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
