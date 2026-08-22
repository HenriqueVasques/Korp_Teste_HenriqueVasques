using AutoMapper;
using Billing.API.DTOs.InvoiceItems;
using Billing.API.Models;

namespace Billing.API.Mappings;

public class InvoiceItemProfile : Profile
{
    public InvoiceItemProfile()
    {
        CreateMap<InvoiceItemCreateDto, InvoiceItem>();

        CreateMap<InvoiceItemUpdateDto, InvoiceItem>();

        CreateMap<InvoiceItem, InvoiceItemResponseDto>();
    }
}