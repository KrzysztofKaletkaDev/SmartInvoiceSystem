using SmartInvoice.Api.DTOs;

namespace SmartInvoice.Api.Services
{
    public interface IInvoiceService
    {
        Task<InvoiceDTO> AddInvoiceAsync(CreateInvoiceDto createDTO);
    }
}
