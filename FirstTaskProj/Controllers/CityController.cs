using FirstTaskProj.Database;
using FirstTaskProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CityController(ApplicationContext context)
        {
            _context = context;
        }
        
        /// <summary>
        /// Gets all <see cref="City"/> records from the database.
        /// </summary>
        /// <returns>Status and list of <see cref="City"/> records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCity()
        {

            return await _context.Cities.ToListAsync();
        }
        
        /// <summary>
        /// Gets a <see cref="City"/> from the database by ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="City"/> to get.</param>
        /// <returns>Status and requested <see cref="City"/>.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityById(int id)
        {
            var existing = await _context.Cities.FindAsync(id);
            if (existing == null)
            {
                return NotFound(new { error = $"City with ID {id} was not found." });
            }
            return Ok(existing);
        }

        /// <summary>
        /// Replaces a <see cref="City"/> in the database with <paramref name="newCity"/>.
        /// </summary>
        /// <param name="newCity"><see cref="City"/> data used to update an existing record.</param>
        /// <returns>Status and updated <see cref="City"/>.</returns>
        [HttpPut]
        public async Task<ActionResult<IEnumerable<City>>> PutCity(City newCity)
        {
            if (newCity == null)
            {
                return BadRequest(new { error = "City payload cannot be null." });
            }

            var existing = await _context.Cities.FindAsync(newCity.Id);

            if (existing == null)
            {
                return NotFound(new { error = $"City with ID {newCity.Id} was not found." });
            }

            _context.Cities.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        /// <summary>
        /// Adds a new <see cref="City"/> to the database.
        /// </summary>
        /// <param name="newCity"><see cref="City"/> to add to the database.</param>
        /// <returns>Status and created <see cref="City"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<City>>> PostCity(City newCity)
        {
            if (newCity == null)
            {
                return BadRequest(new { error = "City payload cannot be null." });
            }

            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();

            return Ok(newCity);
        }

        /// <summary>
        /// Deletes a <see cref="City"/> from the database by ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="City"/> to delete.</param>
        /// <returns>Status and deleted <see cref="City"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> DeleteCity(int id)
        {
            var cityToDelete = await _context.Cities.FindAsync(id);

            if (cityToDelete == null)
            {
                return NotFound(new { error = $"City with ID {id} was not found." });
            }

            _context.Cities.Remove(cityToDelete);
            await _context.SaveChangesAsync();

            return Ok(cityToDelete);
        }
    }
}
