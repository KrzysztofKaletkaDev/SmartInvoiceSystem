using Microsoft.AspNetCore.Mvc;
using SmartInvoice.Api.Models;
using Microsoft.EntityFrameworkCore;
using SmartInvoice.Api.Data;

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
        public async Task<ActionResult<Invoice>> PostInvoice(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoices), new { id = invoice.Id }, invoice);
        }
    }
}
