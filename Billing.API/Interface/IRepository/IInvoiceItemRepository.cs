using Billing.API.Models;

namespace Billing.API.Interface.IRepository
{
    public interface IInvoiceItemRepository
    {
        Task Add(InvoiceItem invoiceItem);
        Task Update(InvoiceItem invoiceItem);
        Task<InvoiceItem?> GetById(int id);
        Task<bool> ExistsProductCodeInInvoice(string productCode, int invoiceId, int currentItemId)
        Task<bool> SaveChangesAsync();
    }
}
