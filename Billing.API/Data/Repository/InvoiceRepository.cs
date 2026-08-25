using Billing.API.Data.Context;
using Billing.API.Interface.IRepository;
using Billing.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Billing.API.Data.Repository
{
    public class InvoiceRepository : IInvoiceRepository
    {
        readonly AppDbContext _appDbContext;
        public InvoiceRepository(AppDbContext appDbContext) 
        {
            _appDbContext = appDbContext;
        }

        public async Task Add(Invoice invoice)
        {
            await _appDbContext.Invoices.AddAsync(invoice);
            await _appDbContext.SaveChangesAsync();
        }

        public async Task Update(Invoice invoice)
        {
            _appDbContext.Invoices.Update(invoice);
            await Task.CompletedTask;
        }

        public async Task<int> GetMaxNumberAsync()
        {
            return await _appDbContext.Invoices
                .MaxAsync(i => (int?)i.Number) ?? 0;
        }

        public async Task<Invoice?> GetById(int invoiceId)
        {
            return await _appDbContext.Invoices
                .Include(i => i.Items)
                .Where(i => i.Id == invoiceId && !i.IsDeleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Invoice>> GetAll()
        {
            return await _appDbContext.Invoices
                .Include(i => i.Items.Where(item => !item.IsDeleted))
                .Where(i => !i.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _appDbContext.SaveChangesAsync() > 0;
        }
    }
}
