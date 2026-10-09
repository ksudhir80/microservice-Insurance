using CustomerService.Data;
using CustomerService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly CustomerDbContext _context;

    public CustomersController(CustomerDbContext context)
    {
        _context = context;
    }

    // GET: api/customers
    [HttpGet]
    public async Task<IActionResult> GetCustomers()
    {
        var customers = await _context.Customers.ToListAsync();
        return Ok(customers);
    }

    // GET: api/customers/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} not found."
            });
        }

        return Ok(customer);
    }

    // POST: api/customers
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetCustomer),
            new { id = customer.Id },
            customer
        );
    }

    // PUT: api/customers/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(
        int id,
        [FromBody] Customer customer)
    {
        if (id != customer.Id)
        {
            return BadRequest(new
            {
                message = "Customer ID in URL does not match customer ID."
            });
        }

        var existingCustomer = await _context.Customers.FindAsync(id);

        if (existingCustomer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} not found."
            });
        }

        existingCustomer.FirstName = customer.FirstName;
        existingCustomer.LastName = customer.LastName;
        existingCustomer.Email = customer.Email;
        existingCustomer.Phone = customer.Phone;
        existingCustomer.DateOfBirth = customer.DateOfBirth;
        existingCustomer.Address = customer.Address;
        existingCustomer.City = customer.City;
        existingCustomer.State = customer.State;
        existingCustomer.ZipCode = customer.ZipCode;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/customers/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);

        if (customer == null)
        {
            return NotFound(new
            {
                message = $"Customer with ID {id} not found."
            });
        }

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}