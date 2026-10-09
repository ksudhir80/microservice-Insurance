using AutoQuoteService.Data;
using AutoQuoteService.Models;
using AutoQuoteService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutoQuoteService.Controllers;

[ApiController]
[Route("api/quotes")]
public class QuotesController : ControllerBase
{
    private readonly IQuoteService _quoteService;
    private readonly QuoteDbContext _dbContext;

    public QuotesController(
        IQuoteService quoteService,
        QuoteDbContext dbContext)
    {
        _quoteService = quoteService;
        _dbContext = dbContext;
    }

    [HttpPost("auto")]
    public async Task<IActionResult> CreateAutoQuote(
        [FromBody] AutoQuoteRequest request)
    {
        if (request.CustomerId <= 0)
        {
            return BadRequest(new
            {
                message = "CustomerId must be greater than zero."
            });
        }

        var quote =
            await _quoteService.CreateAutoQuoteAsync(request);

        return Ok(quote);
    }

    [HttpGet]
    public async Task<IActionResult> GetQuotes()
    {
        var quotes = await _dbContext.AutoQuotes
            .OrderByDescending(q => q.CreatedAt)
            .ToListAsync();

        return Ok(quotes);
    }

    [HttpGet("{quoteId}")]
    public async Task<IActionResult> GetQuote(
        string quoteId)
    {
        var quote = await _dbContext.AutoQuotes
            .FirstOrDefaultAsync(
                q => q.QuoteId == quoteId);

        if (quote == null)
        {
            return NotFound(new
            {
                message = $"Quote {quoteId} not found."
            });
        }

        return Ok(quote);
    }
}