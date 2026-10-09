using AutoQuoteService.Models;
using Microsoft.EntityFrameworkCore;

namespace AutoQuoteService.Data;

public class QuoteDbContext : DbContext
{
    public QuoteDbContext(
        DbContextOptions<QuoteDbContext> options)
        : base(options)
    {
    }

    public DbSet<AutoQuote> AutoQuotes =>
        Set<AutoQuote>();
}