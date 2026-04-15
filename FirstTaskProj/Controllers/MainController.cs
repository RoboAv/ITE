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

        [HttpGet("country/allPag")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllPagination(int page = 1, int pageSize = 10)
        {
            var query = _context.Set<FullCountryView>();

            var totalCount = await query.CountAsync();

            var data = await query.Skip((page - 1) * pageSize).Take(pageSize).ToArrayAsync();
            return Ok(new
            {
                page,
                pageSize,
                totalCount,
                data
            });
        }

        [HttpPut("city/{id}")]
        public async Task<ActionResult<IEnumerable<City>>> PutCity (int id, City newCity)
        {
            var cityToReplace = await _context.Cities.FindAsync(id);

            if (cityToReplace == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(cityToReplace);
            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("region/{id}")]
        public async Task<ActionResult<IEnumerable<Region>>> PutRegion (int id, Region newRegion)
        {
            var regionToReplace = await _context.Regions.FindAsync(id);

            if (regionToReplace == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionToReplace);
            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("country/{id}")]
        public async Task<ActionResult<IEnumerable<Country>>> PutCountry (int id, Country newCountry)
        {
            var countryToReplace = await _context.Countries.FindAsync(id);

            if (countryToReplace == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(countryToReplace);
            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("city")]
        public async Task<ActionResult<IEnumerable<City>>> PostCity (City newCity)
        {
            _context.Cities.Add(newCity);
            await _context.SaveChangesAsync();
            return Ok(newCity);
        }

        [HttpPost("region")]
        public async Task<ActionResult<IEnumerable<Region>>> PostCity (Region newRegion)
        {
            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();
            return Ok(newRegion);
        }

        [HttpPost("country")]
        public async Task<ActionResult<IEnumerable<Country>>> PostCountry (Country newCountry)
        {
            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();
            return Ok(newCountry);
        }

        [HttpDelete("city/{id}")]
        public async Task<ActionResult<IEnumerable<City>>> DeleteCity (int id)
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
        public async Task<ActionResult<IEnumerable<Region>>> DeleteRegion (int id)
        {
            var regionToDelete = await _context.Regions.FindAsync(id);

            if (regionToDelete == null)
            {
                return NotFound();
            }

            _context.Regions.Remove(regionToDelete);
            await _context.SaveChangesAsync();

            return Ok(regionToDelete);
        }

        [HttpDelete("/country/{id}")]
        public async Task<ActionResult<IEnumerable<Country>>> DeleteCountry (int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);

            if (countryToDelete == null)
            {
                return NotFound();
            }

            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();

            return Ok(countryToDelete);
        }
    }
}
