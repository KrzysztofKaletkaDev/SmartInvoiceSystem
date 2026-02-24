using SmartInvoice.Api.Data;
using SmartInvoice.Api.DTOs;
using SmartInvoice.Api.Models;

namespace SmartInvoice.Api.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _context;
        public InvoiceService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<InvoiceDTO> AddInvoiceAsync(CreateInvoiceDto createDTO)
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

            return new InvoiceDTO
            {
                Id = newInvoice.Id,
                InvoiceNumber = newInvoice.InvoiceNumber,
                ContractorName = newInvoice.ContractorName,
                NetAmount = newInvoice.NetAmount,
                GrossAmount = newInvoice.GrossAmount,
                IssueDate = newInvoice.IssueDate,
                OcrContent = newInvoice.OcrContent
            };
        }
    }
}
