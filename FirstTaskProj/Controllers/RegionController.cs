using FirstTaskProj.Database;
using FirstTaskProj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FirstTaskProj.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public RegionController(ApplicationContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets all <see cref="Region"/> records from the database.
        /// </summary>
        /// <returns>Status and list of <see cref="Region"/> records.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegion()
        {

            return await _context.Regions.ToListAsync();
        }

        /// <summary>
        /// Replaces a <see cref="Region"/> in the database with <paramref name="newRegion"/>.
        /// </summary>
        /// <param name="newRegion"><see cref="Region"/> data used to update an existing record.</param>
        /// <returns>Status and updated <see cref="Region"/>.</returns>
        [HttpPut]
        public async Task<ActionResult<IEnumerable<Region>>> PutRegion(Region newRegion)
        {

            if (newRegion == null)
            {
                return BadRequest(new { error = "Region payload cannot be null." });
            }

            var existing = await _context.Regions.FindAsync(newRegion.Id);

            if (existing == null)
            {
                return NotFound(new { error = $"Region with ID {newRegion.Id} was not found." });
            }

            _context.Regions.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        /// <summary>
        /// Adds a new <see cref="Region"/> to the database.
        /// </summary>
        /// <param name="newRegion"><see cref="Region"/> to add to the database.</param>
        /// <returns>Status and created <see cref="Region"/>.</returns>
        [HttpPost]
        public async Task<ActionResult<IEnumerable<Region>>> PostRegion(Region newRegion)
        {
            if (newRegion == null)
            {
                return BadRequest(new { error = "Region payload cannot be null." });
            }

            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();

            return Ok(newRegion);
        }

        /// <summary>
        /// Deletes a <see cref="Region"/> from the database by ID.
        /// </summary>
        /// <param name="id">ID of the <see cref="Region"/> to delete.</param>
        /// <returns>Status and deleted <see cref="Region"/>.</returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Region>>> DeleteRegion(int id)
        {
            var regionToDelete = await _context.Regions.FindAsync(id);

            if (regionToDelete == null)
            {
                return NotFound(new { error = $"Region with ID {id} was not found." });
            }

            _context.Regions.Remove(regionToDelete);
            await _context.SaveChangesAsync();

            return Ok(regionToDelete);
        }
    }
}
