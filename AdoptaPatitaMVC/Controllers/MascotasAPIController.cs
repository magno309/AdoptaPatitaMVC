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
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class MascotasAPIController : ControllerBase
    {
        private readonly AdoptaPatitaContext _context;

        public MascotasAPIController(AdoptaPatitaContext context)
        {
            _context = context;            
        }

        // GET: api/MascotasAPI
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Mascota>>> GetMascotas()
        {
            string refugioId = ObtenerRefugioId().Result;
            if(refugioId != null){
                var mascotas = from m in _context.Mascotas select m;
                mascotas = mascotas.Where(m => m.Id_Refugio == refugioId);
                return await mascotas.ToListAsync();
            }
            return Unauthorized();
            /*var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {                
                IEnumerable<Claim> claims = identity.Claims;
                foreach(Claim c in claims){
                    Console.WriteLine(c.ToString());
                }
                var email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                if(email != null){                    
                    //string refugioId = _context.Refugios.FirstOrDefault(r => r.Email == email.Value).RefugioId.ToString();
                    var mascotas = from m in _context.Mascotas select m;
                    mascotas = mascotas.Where(m => m.Id_Refugio == refugioId);
                    return await mascotas.ToListAsync();
                }
                else
                    Console.WriteLine("EMAIL NULL");
            }
            else
                Console.WriteLine("USER NULL");
            return NoContent();*/
        }

        // GET: api/MascotasAPI/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Mascota>> GetMascota(int id)
        {            
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            string refugioId = ObtenerRefugioId().Result;
            if(mascota.Id_Refugio != refugioId){
                return Unauthorized();
            }
            return mascota;
        }

        // PUT: api/MascotasAPI/2
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMascota(int id, Mascota mascota)
        {
            if (id != mascota.MascotaId)
            {
                return BadRequest();
            }

            string refugioId = ObtenerRefugioId().Result;
            if(mascota.Id_Refugio != refugioId){
                return Unauthorized();
            }

            _context.Entry(mascota).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MascotaExists(id))
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

        // POST: api/MascotasAPI
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Mascota>> PostMascota(Mascota mascota)
        {
            string refugioId = ObtenerRefugioId().Result;
            if(mascota.Id_Refugio != refugioId){
                return Unauthorized();
            }
            _context.Mascotas.Add(mascota);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMascota), new { id = mascota.MascotaId }, mascota);
        }

        // DELETE: api/MascotasAPI/2
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMascota(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }

            string refugioId = ObtenerRefugioId().Result;
            if(mascota.Id_Refugio != refugioId){
                return Unauthorized();
            }

            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.MascotaId == id);
        }

        private async Task<string> ObtenerRefugioId(){
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {                
                IEnumerable<Claim> claims = identity.Claims;
                /*foreach(Claim c in claims){
                    Console.WriteLine(c.ToString());
                }*/
                var email = identity.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress");
                if(email != null){                    
                    var refugio = await _context.Refugios.FirstOrDefaultAsync(r => r.Email == email.Value);
                    return refugio.RefugioId.ToString();                     
                }
                else
                    Console.WriteLine("EMAIL NULL");
            }
            else
                Console.WriteLine("USER NULL");
            return null;
        }
    }
}
