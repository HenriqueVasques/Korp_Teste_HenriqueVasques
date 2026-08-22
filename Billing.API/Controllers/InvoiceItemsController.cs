using Billing.API.DTOs.InvoiceItems;
using Billing.API.Interface.IService;
using Billing.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace Billing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceItemsController : ControllerBase
    {
        readonly IInvoiceItemService _invoiceItemService;

        public InvoiceItemsController(IInvoiceItemService invoiceItemService)
        {
            _invoiceItemService = invoiceItemService;
        }

        [HttpPost("{invoiceId:int}/items")]
        public async Task<IActionResult> CreateInvoiceItem([FromBody] InvoiceItemCreateDto dto, int invoiceId)
        {
            try
            {
                var item = await _invoiceItemService.CreateInvoiceItem(dto, invoiceId);
                return StatusCode(StatusCodes.Status201Created, new { message = "Item adicionado com sucesso!", dados = item });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ocorreu um erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpPut("{invoiceId:int}/items/{invoiceItemId:int}")]
        public async Task<IActionResult> UpdateInvoiceItem( [FromBody] InvoiceItemUpdateDto dto, int invoiceId, int invoiceItemId)
        {
            try
            {
                var item = await _invoiceItemService.UpdateInvoiceItem(invoiceId, invoiceItemId, dto);
                return Ok(new { message = "Item atualizado com sucesso!", dados = item });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ocorreu um erro interno no servidor.", detail = ex.Message });
            }
        }

        [HttpDelete("{invoiceId:int}/items/{invoiceItemId:int}")]
        public async Task<IActionResult> DeleteInvoiceItem(int invoiceId, int invoiceItemId)
        {
            try
            {
                await _invoiceItemService.DeleteInvoiceItem(invoiceId, invoiceItemId);
                return Ok(new { message = "Item da nota fiscal removido com sucesso!" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Ocorreu um erro interno no servidor.", detail = ex.Message });
            }
        }
    }
}
