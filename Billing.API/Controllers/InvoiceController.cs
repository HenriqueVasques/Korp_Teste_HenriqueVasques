using Billing.API.DTOs.Invoices;
using Billing.API.Interface.IService;
using Microsoft.AspNetCore.Mvc;

namespace Billing.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreateDto dto)
        {
            try
            {
                var invoice = await _invoiceService.CreateInvoice(dto);
                return StatusCode(StatusCodes.Status201Created, new { message = "Nota Fiscal criada com sucesso!", dados = invoice });
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

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            try
            {
                await _invoiceService.DeleteInvoice(id);
                return Ok(new { message = "Nota Fiscal deletada com sucesso!" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
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

        [HttpPut("{id:int}/close")]
        public async Task<IActionResult> CloseInvoice(int id)
        {
            try
            {
                await _invoiceService.CloseInvoice(id); // ou o nome do seu método no Service
                return Ok(new { message = "Nota Fiscal fechada e estoque abatido com sucesso!" });
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
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erro ao fechar a nota fiscal.", detail = ex.Message });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var result = await _invoiceService.GetById(id);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erro interno ao buscar a nota fiscal.", detail = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _invoiceService.GetAll();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { error = "Erro interno ao listar as notas fiscais.", detail = ex.Message });
            }
        }

    }
}