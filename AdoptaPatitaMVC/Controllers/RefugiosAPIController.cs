using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Models;
using AdoptaPatitaMVC.Data;
using Microsoft.AspNetCore.Authorization;
using System;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RefugiosAPIController : ControllerBase
    {
        private readonly AdoptaPatitaContext _context;

        public RefugiosAPIController(AdoptaPatitaContext context)
        {
            _context = context;
        }

        // GET: api/RefugiosAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Refugio>>> GetRefugios()
        {
            Console.WriteLine("UTC: "+DateTime.UtcNow);
            return await _context.Refugios.ToListAsync();
        }

        // GET: api/RefugiosAPI/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Refugio>> GetRefugio(int id)
        {
            var refugio = await _context.Refugios.FindAsync(id);

            if (refugio == null)
            {
                return NotFound();
            }

            return refugio;
        }

        // PUT: api/RefugiosAPI/2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRefugio(int id, Refugio refugio)
        {
            if (id != refugio.RefugioId)
            {
                return BadRequest();
            }

            _context.Entry(refugio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RefugioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/RefugiosAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Refugio>> PostRefugio(Refugio refugio)
        {
            _context.Refugios.Add(refugio);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRefugio), new { id = refugio.RefugioId }, refugio);
        }

        // DELETE: api/RefugiosAPI/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRefugio(int id)
        {
            var refugio = await _context.Refugios.FindAsync(id);
            if (refugio == null)
            {
                return NotFound();
            }

            _context.Refugios.Remove(refugio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RefugioExists(int id)
        {
            return _context.Refugios.Any(e => e.RefugioId == id);
        }
    }
}
