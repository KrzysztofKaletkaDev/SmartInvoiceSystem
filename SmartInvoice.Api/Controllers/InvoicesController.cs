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
        private readonly OcrService _ocrService;

        public InvoicesController(IInvoiceService invoiceService, AppDbContext context, OcrService ocrService)
        {
            _invoiceService = invoiceService;
            _context = context;
            _ocrService = ocrService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadInvoices(IFormFile file)
        {
            if (file == null || file.Length == 0) return BadRequest("Nie przesłano pliku.");

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);
            var fileBytes = ms.ToArray();

            var aiResult = await _ocrService.AnalyzeInvoiceAsync(fileBytes);
            if (aiResult == null) return StatusCode(500, "AI nie zdołało przeanalizować dokumentu.");

            var invoice = new Invoice()
            {
                InvoiceNumber = aiResult.Number ?? "Brak numeru",
                ContractorName = aiResult.Contractor ?? "Brak nazwy kontrahenta",
                NetAmount = aiResult.NetAmount,
                GrossAmount = aiResult.GrossAmount,
                IssueDate = DateTime.TryParse(aiResult.Date, out var date) ? date : DateTime.Now,
                OcrContent = $"Sparsowano przez AI. Plik: {file.FileName}"
            };
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoices), new { id = invoice.Id }, invoice);


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