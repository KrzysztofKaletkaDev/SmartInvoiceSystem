using Microsoft.AspNetCore.Mvc;
using SmartInvoice.Api.Models;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.Api.Data;
using SmartInvoice.Api.DTOs;

namespace SmartInvoice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }
        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> PostInvoice(CreateInvoiceDto createDTO)
        {
            var newInvoice = new Invoice
            {
                InvoiceNumber = createDTO.InvoiceNumber,
                ContractorName = createDTO.ContractorName,
                NetAmount = createDTO.NetAmount,
                GrossAmount = createDTO.GrossAmount,
                IssueDate = createDTO.IssueDate
            };
            _context.Invoices.Add(newInvoice);
            await _context.SaveChangesAsync();
            var responseDTO = new InvoiceDTO
            {
                Id = newInvoice.Id,
                InvoiceNumber = newInvoice.InvoiceNumber,
                ContractorName = newInvoice.ContractorName,
                NetAmount = newInvoice.NetAmount,
                GrossAmount = newInvoice.GrossAmount,
                IssueDate = newInvoice.IssueDate,
                OcrContent = newInvoice.OcrContent
            };
            return CreatedAtAction(nameof(GetInvoices), new { id = responseDTO.Id }, responseDTO);
        }
    }
}
