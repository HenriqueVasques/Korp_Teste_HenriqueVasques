using AutoMapper;
using Billing.API.DTOs.InvoiceItems;
using Billing.API.Interface.IRepository;
using Billing.API.Interface.IService;
using Billing.API.Models;
using static Billing.API.Enum.InvoiceStatusEnum;

namespace Billing.API.Service
{
    public class InvoiceItemsService : IInvoiceItemService
    {
        private readonly IInvoiceItemRepository _iInvoiceItemRepository;
        private readonly IInvoiceRepository _iInvoiceRepository;
        private readonly IMapper _mapper;

        public InvoiceItemsService(
            IInvoiceItemRepository iInvoiceItemRepository,
            IInvoiceRepository iInvoiceRepository,
            IMapper mapper)
        {
            _iInvoiceItemRepository = iInvoiceItemRepository;
            _iInvoiceRepository = iInvoiceRepository;
            _mapper = mapper;
        }

        public async Task<InvoiceItemResponseDto> CreateInvoiceItem(InvoiceItemCreateDto dto, int invoiceId)
        {
            var invoice = await _iInvoiceRepository.GetById(invoiceId);

            if (invoice == null)
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");


            if (invoice.Status != InvoiceStatus.Open)
                throw new InvalidOperationException("Não é possível adicionar itens a uma nota fiscal que não esteja aberta.");

            var invoiceItem = _mapper.Map<InvoiceItem>(dto);
            invoiceItem.InvoiceId = invoiceId;

            await _iInvoiceItemRepository.Add(invoiceItem);

            invoice.TotalAmount += invoiceItem.Total;

            await _iInvoiceItemRepository.SaveChangesAsync();

            return _mapper.Map<InvoiceItemResponseDto>(invoiceItem);
        }

        public async Task<InvoiceItemResponseDto> UpdateInvoiceItem(int invoiceId, int invoiceItemId, InvoiceItemUpdateDto dto)
        {
            var invoice = await _iInvoiceRepository.GetById(invoiceId);
            if (invoice == null)
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");

            if (invoice.Status != InvoiceStatus.Open)
                throw new InvalidOperationException("Não é possível alterar itens de uma nota fiscal que não esteja aberta.");

            var invoiceItem = await _iInvoiceItemRepository.GetById(invoiceItemId);
            if (invoiceItem == null)
                throw new KeyNotFoundException("Item da nota fiscal não encontrado.");

            if (invoiceItem.InvoiceId != invoiceId)
                throw new InvalidOperationException("O item da nota fiscal não pertence à nota fiscal especificada.");

            var isProductCodeInUseByAnotherItem = await _iInvoiceItemRepository.ExistsProductCodeInInvoice(dto.ProductCode, invoiceId, invoiceItemId);
            if (isProductCodeInUseByAnotherItem)
                throw new InvalidOperationException("Já existe outro item com este código de produto nesta nota fiscal.");

            var oldItemTotal = invoiceItem.Total;

            _mapper.Map(dto, invoiceItem);

            invoice.TotalAmount += (invoiceItem.Total - oldItemTotal);

            await _iInvoiceItemRepository.Update(invoiceItem);

            return _mapper.Map<InvoiceItemResponseDto>(invoiceItem);
        }

        public async Task DeleteInvoiceItem(int invoiceId, int invoiceItemId)
        {
            var invoice = await _iInvoiceRepository.GetById(invoiceId);
            if (invoice == null)
                throw new KeyNotFoundException("Nota Fiscal não encontrada.");
            if (invoice.Status != InvoiceStatus.Open)
                throw new InvalidOperationException("Não é possível deletar itens de uma nota fiscal que não esteja aberta.");
            var invoiceItem = await _iInvoiceItemRepository.GetById(invoiceItemId);
            if (invoiceItem == null)
                throw new KeyNotFoundException("Item da nota fiscal não encontrado.");
            if (invoiceItem.InvoiceId != invoiceId)
                throw new InvalidOperationException("O item da nota fiscal não pertence à nota fiscal especificada.");

            invoice.TotalAmount -= invoiceItem.Total;
            invoiceItem.IsDeleted = true;
            await _iInvoiceItemRepository.Update(invoiceItem);
        }
    }
}