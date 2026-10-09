using AutoQuoteService.Models;

namespace AutoQuoteService.Services;

public interface IQuoteService
{
    Task<AutoQuoteResponse> CreateAutoQuoteAsync(
        AutoQuoteRequest request);
}