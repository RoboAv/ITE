using FirstTaskProj.Database;
using FirstTaskProj.Database.Views;
using FirstTaskProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CountryController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public CountryController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountry()
        {
            return await _context.Countries.ToListAsync();
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountryView()
        {
            var data = await _context.Set<FullCountryView>().ToListAsync();
            return Ok(data);
        }

        [HttpGet("allPag")]
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

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Country>>> PutCountry(Country newCountry)
        {
            var existing = await _context.Countries.FindAsync(newCountry.Id);
            if (existing == null)
            {
                return NotFound();
            }

            existing.Name = newCountry.Name;
            existing.LeaderName = newCountry.LeaderName;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Country>>> PostCountry(Country newCountry)
        {
            if (newCountry == null)
            {
                return BadRequest();
            }

            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();

            return Ok(newCountry);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Country>>> DeleteCountry(int id)
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
