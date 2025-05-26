using Microsoft.AspNetCore.Mvc;
using Cinema.Infrastructure.Models;
using Cinema.Infrastructure.Services;
using Cinema.REST.Models;

namespace Cinema.REST.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly ICrudServiceAsync<Employee> _service;

    public EmployeesController(ICrudServiceAsync<Employee> service) => _service = service;

    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await _service.ReadAllAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var employee = await _service.ReadAsync(id);
        return employee == null ? NotFound() : Ok(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeModel model)
    {
        var employee = new Employee
        {
            Name = model.Name,
            Position = model.Position,
            HireDate = model.HireDate
        };
        await _service.CreateAsync(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, EmployeeModel model)
    {
        var employee = await _service.ReadAsync(id);
        if (employee == null) return NotFound();

        employee.Name = model.Name;
        employee.Position = model.Position;
        employee.HireDate = model.HireDate;

        await _service.UpdateAsync(employee);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var employee = await _service.ReadAsync(id);
        if (employee == null) return NotFound();

        await _service.RemoveAsync(employee);
        return NoContent();
    }
}
