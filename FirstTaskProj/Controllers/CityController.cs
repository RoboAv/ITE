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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCity()
        {

            return await _context.Cities.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCityById(int id)
        {
            var existing = await _context.Cities.FindAsync(id);
            if (existing == null)
            {
                return NotFound();
            }
            return Ok(existing);
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<City>>> PutCity(City newCity)
        {
            if (newCity == null)
            {
                return BadRequest();
            }

            var existing = await _context.Cities.FindAsync(newCity.Id);

            if (existing == null)
            {
                return NotFound();
            }

            _context.Cities.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<City>>> PostCity(City newCity)
        {
            if (newCity == null)
            {
                return BadRequest();
            }

            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();

            return Ok(newCity);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> DeleteCity(int id)
        {
            var cityToDelete = await _context.Cities.FindAsync(id);

            if (cityToDelete == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(cityToDelete);
            await _context.SaveChangesAsync();

            return Ok(cityToDelete);
        }
    }
}
