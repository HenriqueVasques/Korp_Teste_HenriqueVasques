using AutoMapper;
using Billing.API.DTOs.InvoiceItems;
using Billing.API.DTOs.Invoices;
using Billing.API.Models;

public class InvoiceProfile : Profile
{
    public InvoiceProfile()
    {
        CreateMap<InvoiceCreateDto, Invoice>();
        CreateMap<InvoiceItemCreateDto, InvoiceItem>();
        CreateMap<Invoice, InvoiceResponseDto>();
        CreateMap<InvoiceItem, InvoiceItemResponseDto>();
    }
}