using AutoMapper;
using Billing.API.DTOs.Invoices;
using Billing.API.Models;

namespace Billing.API.Mappings;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<Invoice, InvoiceResponseDto>();
    }
}