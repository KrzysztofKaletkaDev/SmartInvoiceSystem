using System.ComponentModel.DataAnnotations;

namespace SmartInvoice.Api.Models
{
    public class Invoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string InvoiceNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ContractorName { get; set; } = string.Empty;

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "NetAmount must be greater than 0")]
        public decimal NetAmount { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "GrossAmount must be greater than 0")]
        public decimal GrossAmount { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [StringLength(5000)]
        public string? OcrContent { get; set; }
    }
}
