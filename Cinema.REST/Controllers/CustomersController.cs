using Microsoft.AspNetCore.Mvc;
using Cinema.Infrastructure.Models;
using Cinema.Infrastructure.Services;
using Cinema.REST.Models;

namespace Cinema.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ICrudServiceAsync<Customer> _service;

    public CustomersController(ICrudServiceAsync<Customer> service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.ReadAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var customer = await _service.ReadAsync(id);
        return customer == null ? NotFound() : Ok(customer);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CustomerModel model)
    {
        var customer = new Customer
        {
            Name = model.Name,
            Age = model.Age,
            FavoriteGenre = model.FavoriteGenre
        };
        await _service.CreateAsync(customer);
        return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, CustomerModel model)
    {
        var customer = await _service.ReadAsync(id);
        if (customer == null) return NotFound();

        customer.Name = model.Name;
        customer.Age = model.Age;
        customer.FavoriteGenre = model.FavoriteGenre;

        await _service.UpdateAsync(customer);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var customer = await _service.ReadAsync(id);
        if (customer == null) return NotFound();

        await _service.RemoveAsync(customer);
        return NoContent();
    }
}
