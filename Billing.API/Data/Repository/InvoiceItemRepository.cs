using Billing.API.Data.Context;
using Billing.API.Interface.IRepository;
using Billing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.API.Data.Repository
{
    public class InvoiceItemRepository : IInvoiceItemRepository
    {
        private readonly AppDbContext _appDbContext;

        public InvoiceItemRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public Task Add(InvoiceItem invoiceItem)
        {
            _appDbContext.InvoiceItems.Add(invoiceItem);
            return Task.CompletedTask;
        }

        public async Task Update(InvoiceItem invoiceItem)
        {
             _appDbContext.InvoiceItems.Update(invoiceItem);
            await SaveChangesAsync();
        }

        public async Task<InvoiceItem?> GetById(int id)
        {
            return await _appDbContext.InvoiceItems
                .Where(i => i.Id == id && !i.IsDeleted)
                .FirstOrDefaultAsync();
        }
        public async Task<bool> ExistsProductCodeInInvoice(string productCode, int invoiceId, int currentItemId)
        {
            return await _appDbContext.InvoiceItems
                .AnyAsync(i => i.ProductCode == productCode
                            && i.InvoiceId == invoiceId
                            && i.Id != currentItemId
                            && !i.IsDeleted
                );
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
