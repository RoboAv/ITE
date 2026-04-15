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

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Region>>> GetRegion()
        {

            return await _context.Regions.ToListAsync();
        }

        [HttpPut]
        public async Task<ActionResult<IEnumerable<Region>>> PutRegion(Region newRegion)
        {

            if (newRegion == null)
            {
                return BadRequest();
            }

            var existing = await _context.Regions.FindAsync(newRegion.Id);

            if (existing == null)
            {
                return NotFound();
            }

            _context.Regions.Update(existing);
            await _context.SaveChangesAsync();

            return Ok(existing);
        }

        [HttpPost]
        public async Task<ActionResult<IEnumerable<Region>>> PostRegion(Region newRegion)
        {
            if (newRegion == null)
            {
                return BadRequest();
            }

            _context.Regions.Add(newRegion);
            await _context.SaveChangesAsync();

            return Ok(newRegion);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<IEnumerable<Region>>> DeleteRegion(int id)
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
    }
}
