using Microsoft.EntityFrameworkCore;
using SmartInvoice.Api.Models;

namespace SmartInvoice.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Invoice> Invoices { get; set; }
}
