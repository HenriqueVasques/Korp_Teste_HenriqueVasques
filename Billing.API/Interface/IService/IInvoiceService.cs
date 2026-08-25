using Billing.API.DTOs.Invoices;
using Billing.API.Models;

namespace Billing.API.Interface.IService
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> CreateInvoice(InvoiceCreateDto dto);
        Task DeleteInvoice(int invoiceId);
        Task<InvoiceResponseDto?> GetById(int invoiceId);
        Task<IEnumerable<InvoiceResponseDto>> GetAll();
        Task<InvoiceResponseDto> CloseInvoice(int id);
    }
}
