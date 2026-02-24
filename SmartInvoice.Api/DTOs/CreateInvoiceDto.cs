namespace SmartInvoice.Api.DTOs
{
    public class CreateInvoiceDto
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public string ContractorName { get; set; } = string.Empty;
        public decimal NetAmount { get; set; }  
        public decimal GrossAmount { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
