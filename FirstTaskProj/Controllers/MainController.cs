using FirstTaskProj.Database;
using FirstTaskProj.Database.Views;
using FirstTaskProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class MainController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public MainController(ApplicationContext context) {
            _context = context;
        }

        [HttpGet("city")]
        public async Task<ActionResult<IEnumerable<City>>> GetCity()
        {

            return await _context.Cities.ToListAsync();
        }

        [HttpGet("region")]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegion()
        {

            return await _context.Regions.ToListAsync();
        }

        [HttpGet("country")]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountry()
        {
            return await _context.Countries.ToListAsync();
        }

        [HttpGet("country/all")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountryView()
        {
            var data = await _context.Set<FullCountryView>().ToListAsync();
            return Ok(data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> Get(int id)
        {
            var city = await _context.Cities.FindAsync(id);
            if (city == null)
            {
                return NotFound();
            }
            return Ok(city);
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> Post(int id)
        {
            //return await _context.Cities.ToListAsync();
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<City>>> Put(int id, City city)
        {
            var cityToDelete = await _context.Cities.FindAsync(id);

            if (cityToDelete == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(cityToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("city/{id}")]
        public async Task<ActionResult<IEnumerable<City>>> CityDelete(int id)
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

        [HttpDelete("/region/{id}")]
        public async Task<ActionResult<IEnumerable<Region>>> RegionDelete(int id)
        {
            var regionToDelete = await _context.Regions.FindAsync(id);

            if (regionToDelete == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("/country/{id}")]
        public async Task<ActionResult<IEnumerable<Country>>> CountryDelete(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);

            if (countryToDelete == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
