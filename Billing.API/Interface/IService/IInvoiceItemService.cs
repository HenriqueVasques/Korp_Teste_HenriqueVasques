using Billing.API.DTOs.InvoiceItems;
using Billing.API.Models;

namespace Billing.API.Interface.IService
{
    public interface IInvoiceItemService
    {
        Task<InvoiceItemResponseDto> CreateInvoiceItem(InvoiceItemCreateDto dto, int invoiceId);
        Task<InvoiceItemResponseDto> UpdateInvoiceItem(int invoiceId, int invoiceItemId, InvoiceItemUpdateDto dto);
        Task DeleteInvoiceItem(int invoiceId, int invoiceItemId);
    }
}
