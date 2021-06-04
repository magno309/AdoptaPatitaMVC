using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Data;
using AdoptaPatitaMVC.Models;
using Microsoft.AspNetCore.Authorization;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize]
    public class MascotasController : Controller
    {
        private readonly AdoptaPatitaContext _context;

        public MascotasController(AdoptaPatitaContext context)
        {
            _context = context;
        }

        // GET: Mascotas
        
        [AllowAnonymous]
        public async Task<IActionResult> Index(string tamanioMascota, string sexoMascota, string edadMascota)
        {
            IQueryable<string> tamanioQuery = from m in _context.Mascotas
                                              orderby m.Tamanio
                                              select m.Tamanio;

            IQueryable<string> sexoQuery = from m in _context.Mascotas
                                              orderby m.Sexo
                                              select m.Sexo;

            IQueryable<string> edadQuery = from m in _context.Mascotas
                                              orderby m.Edad
                                              select m.Edad;

            var mascotas = from m in _context.Mascotas
                           select m;

            if (!string.IsNullOrEmpty(tamanioMascota))
            {
                mascotas = mascotas.Where(x => x.Tamanio == tamanioMascota);
            }

            if (!string.IsNullOrEmpty(sexoMascota))
            {
                mascotas = mascotas.Where(x => x.Sexo == sexoMascota);
            }

            if (!string.IsNullOrEmpty(edadMascota))
            {
                mascotas = mascotas.Where(x => x.Edad == edadMascota);
            }

            var busquedaMascotaVM = new BusquedaMascotaViewModel
            {
                Tamanios = new SelectList(await tamanioQuery.Distinct().ToListAsync()),
                Edades = new SelectList(await edadQuery.Distinct().ToListAsync()),
                Sexos = new SelectList(await sexoQuery.Distinct().ToListAsync()),
                Mascotas = await mascotas.ToListAsync()
            };

            return View(busquedaMascotaVM);
        }

        // GET: Mascotas/Details/5
        
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .FirstOrDefaultAsync(m => m.MascotaId == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // GET: Mascotas/Create
        [Authorize(Roles = "RefugioRole")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mascotas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> Create([Bind("MascotaId,Nombre,Raza,Color,Sexo,Edad,Peso,Tamanio,Esterilizado,Descripcion,Historia,Imagen1,Imagen2,Imagen3,Id_Refugio")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mascota);
        }

        // GET: Mascotas/Edit/5
        [Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas.FindAsync(id);
            if (mascota == null)
            {
                return NotFound();
            }
            return View(mascota);
        }

        // POST: Mascotas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> Edit(int id, [Bind("MascotaId,Nombre,Raza,Color,Sexo,Edad,Peso,Tamanio,Esterilizado,Descripcion,Historia,Imagen1,Imagen2,Imagen3,Id_Refugio")] Mascota mascota)
        {
            if (id != mascota.MascotaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(mascota);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MascotaExists(mascota.MascotaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(mascota);
        }

        // GET: Mascotas/Delete/5
        [Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var mascota = await _context.Mascotas
                .FirstOrDefaultAsync(m => m.MascotaId == id);
            if (mascota == null)
            {
                return NotFound();
            }

            return View(mascota);
        }

        // POST: Mascotas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var mascota = await _context.Mascotas.FindAsync(id);
            _context.Mascotas.Remove(mascota);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        private bool MascotaExists(int id)
        {
            return _context.Mascotas.Any(e => e.MascotaId == id);
        }
    }
}
