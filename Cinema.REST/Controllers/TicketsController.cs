using Microsoft.AspNetCore.Mvc;
using Cinema.Infrastructure.Models;
using Cinema.Infrastructure.Services;
using Cinema.REST.Models;

namespace Cinema.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TicketsController : ControllerBase
{
    private readonly ICrudServiceAsync<Ticket> _service;

    public TicketsController(ICrudServiceAsync<Ticket> service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.ReadAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var ticket = await _service.ReadAsync(id);
        return ticket == null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TicketModel model)
    {
        var ticket = new Ticket
        {
            MovieId = model.MovieId,
            CustomerId = model.CustomerId,
            ShowTime = model.ShowTime,
            Price = model.Price
        };
        await _service.CreateAsync(ticket);
        return CreatedAtAction(nameof(Get), new { id = ticket.Id }, ticket);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, TicketModel model)
    {
        var ticket = await _service.ReadAsync(id);
        if (ticket == null) return NotFound();

        ticket.MovieId = model.MovieId;
        ticket.CustomerId = model.CustomerId;
        ticket.ShowTime = model.ShowTime;
        ticket.Price = model.Price;

        await _service.UpdateAsync(ticket);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var ticket = await _service.ReadAsync(id);
        if (ticket == null) return NotFound();

        await _service.RemoveAsync(ticket);
        return NoContent();
    }
}
