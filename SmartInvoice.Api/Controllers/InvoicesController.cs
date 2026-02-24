using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.Api.Data;
using SmartInvoice.Api.DTOs;
using SmartInvoice.Api.Models;
using SmartInvoice.Api.Services;

namespace SmartInvoice.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly AppDbContext _context;

        public InvoicesController(IInvoiceService invoiceService, AppDbContext context)
        {
            _invoiceService = invoiceService;
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<InvoiceDTO>> PostInvoice(CreateInvoiceDto createDto)
        {
            var resultDto = await _invoiceService.AddInvoiceAsync(createDto);

            return CreatedAtAction(nameof(GetInvoices), new { id = resultDto.Id }, resultDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoices(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null) return NotFound();
            return invoice;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetAllInvoices()
        {
            return await _context.Invoices.ToListAsync();
        }
    }
}