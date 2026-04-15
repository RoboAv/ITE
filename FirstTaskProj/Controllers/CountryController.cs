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
        
        /// <summary>
        /// Gets all <see cref="Country"/> records from the database.
        /// </summary>
        /// <returns>Status and list of <see cref="Country"/> records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> GetCountry()
        {
            return await _context.Countries.ToListAsync();
        }

        /// <summary>
        /// Gets all <see cref="FullCountryView"/> records from the database.
        /// </summary>
        /// <returns>Status and list of <see cref="FullCountryView"/> records.</returns> 
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountryView()
        {
            var data = await _context.Set<FullCountryView>().ToListAsync();
            return Ok(data);
        }

        /// <summary>
        /// Gets <see cref="FullCountryView"/> records from the database with pagination.
        /// </summary>
        /// <param name="page">Page number starting from 1.</param>
        /// <param name="pageSize">Number of items per page.</param>
        /// <returns>Status and paginated <see cref="FullCountryView"/> records.</returns>
        [HttpGet("allPag")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllPagination(int page = 1, int pageSize = 10)
        {
            if (page < 1 || pageSize < 1)
            {
                return BadRequest(new { error = "Page and pageSize must be greater than 0." });
            }

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

        /// <summary>
        /// Replaces a <see cref="Country"/> in the database with <paramref name="newCountry"/>.
        /// </summary>
        /// <param name="newCountry"><see cref="Country"/> data used to update an existing record.</param>
        /// <returns>Status and updated <see cref="Country"/>.</returns>
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Country>>> PutCountry(Country newCountry)
        {
            if (newCountry == null)
            {
                return BadRequest(new { error = "Country payload cannot be null." });
            }

            var existing = await _context.Countries.FindAsync(newCountry.Id);
            if (existing == null)
            {
                return NotFound(new { error = $"Country with ID {newCountry.Id} was not found." });
            }

            existing.Name = newCountry.Name;
            existing.LeaderName = newCountry.LeaderName;
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        /// <summary>
        /// Adds a new <see cref="Country"/> to the database.
        /// </summary>
        /// <param name="newCountry"><see cref="Country"/> to add to the database.</param>
        /// <returns>Status and created <see cref="Country"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Country>>> PostCountry(Country newCountry)
        {
            if (newCountry == null)
            {
                return BadRequest(new { error = "Country payload cannot be null." });
            }

            _context.Countries.Add(newCountry);
            await _context.SaveChangesAsync();

            return Ok(newCountry);
        }

        /// <summary>
        /// Deletes a <see cref="Country"/> from the database by ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="Country"/> to delete.</param>
        /// <returns>Status and deleted <see cref="Country"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Country>>> DeleteCountry(int id)
        {
            var countryToDelete = await _context.Countries.FindAsync(id);

            if (countryToDelete == null)
            {
                return NotFound(new { error = $"Country with ID {id} was not found." });
            }

            _context.Countries.Remove(countryToDelete);
            await _context.SaveChangesAsync();

            return Ok(countryToDelete);
        }
    }
}
