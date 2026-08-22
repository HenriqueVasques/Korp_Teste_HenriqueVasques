using Billing.API.DTOs.Invoices;
using Billing.API.Models;

namespace Billing.API.Interface.IService
{
    public interface IInvoiceService
    {
        Task<InvoiceResponseDto> CreateInvoice();
        Task DeleteInvoice(int invoiceId);
        Task<InvoiceResponseDto?> GetById(int invoiceId);
        Task<IEnumerable<InvoiceResponseDto>> GetAll();
    }
}
