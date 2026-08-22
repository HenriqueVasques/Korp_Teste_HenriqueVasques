using Billing.API.Models;

namespace Billing.API.Interface.IRepository
{
    public interface IInvoiceRepository
    {
        Task Add(Invoice invoice);
        Task Update(Invoice invoice);
        Task<Invoice?> GetById(int invoiceId);
        Task<int> GetMaxNumberAsync();
        Task<IEnumerable<Invoice>> GetAll();
    }
}
