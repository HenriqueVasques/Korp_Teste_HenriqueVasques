using Billing.API.Models;

namespace Billing.API.Interface.IRepository
{
    public interface IInvoiceItemRepository
    {
        Task Add(InvoiceItem invoiceItem);
        Task Update(InvoiceItem invoiceItem);
        Task<InvoiceItem?> GetById(int id);
        Task<bool> ExistsDescriptionInInvoice(string description, int invoiceId, int currentItemId = 0);
        Task<bool> ExistsProductCodeInInvoice(string productCode, int invoiceId, int currentItemId = 0);
        Task<bool> SaveChangesAsync();
    }
}
