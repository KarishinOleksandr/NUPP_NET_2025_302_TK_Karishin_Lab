using Cinema.REST.Models;
using Microsoft.AspNetCore.Mvc;
using Cinema.Infrastructure.Models;
using Cinema.Infrastructure.Services;

namespace Cinema.REST.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly ICrudServiceAsync<Movie> _service;

        public MoviesController(ICrudServiceAsync<Movie> service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _service.ReadAllAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var movie = await _service.ReadAsync(id);
            return movie == null ? NotFound() : Ok(movie);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] MovieModel model)
        {
            var movie = new Movie
            {
                Title = model.Title,
                Genre = model.Genre,
                Year = model.Year,
                Rating = model.Rating
            };
            await _service.CreateAsync(movie);
            return CreatedAtAction(nameof(Get), new { id = movie.Id }, movie);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] MovieModel model)
        {
            var movie = await _service.ReadAsync(id);
            if (movie == null) return NotFound();

            movie.Title = model.Title;
            movie.Genre = model.Genre;
            movie.Year = model.Year;
            movie.Rating = model.Rating;

            await _service.UpdateAsync(movie);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var movie = await _service.ReadAsync(id);
            if (movie == null) return NotFound();

            await _service.RemoveAsync(movie);
            return NoContent();
        }
    }
}
