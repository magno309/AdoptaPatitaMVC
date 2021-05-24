using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Data;
using AdoptaPatitaMVC.Models;

namespace AdoptaPatitaMVC.Controllers
{
    public class RegistrosAdopcionController : Controller
    {
        private readonly AdoptaPatitaContext _context;

        public RegistrosAdopcionController(AdoptaPatitaContext context)
        {
            _context = context;
        }

        // GET: RegistrosAdopcion
        public async Task<IActionResult> Index()
        {
            var adoptaPatitaContext = _context.RegistrosAdopcion.Include(r => r.Adoptante).Include(r => r.Mascota);
            return View(await adoptaPatitaContext.ToListAsync());
        }

        // GET: RegistrosAdopcion/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroAdopcion = await _context.RegistrosAdopcion
                .Include(r => r.Adoptante)
                .Include(r => r.Mascota)
                .FirstOrDefaultAsync(m => m.RegistroAdopcionId == id);
            if (registroAdopcion == null)
            {
                return NotFound();
            }

            return View(registroAdopcion);
        }

        // GET: RegistrosAdopcion/Create
        public IActionResult Create()
        {
            ViewData["AdoptanteId"] = new SelectList(_context.Adoptante, "AdoptanteId", "AdoptanteId");
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "MascotaId", "MascotaId");
            return View();
        }

        // POST: RegistrosAdopcion/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RegistroAdopcionId,MascotaId,AdoptanteId,FechaAdop,EnumProceso")] RegistroAdopcion registroAdopcion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(registroAdopcion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AdoptanteId"] = new SelectList(_context.Adoptante, "AdoptanteId", "AdoptanteId", registroAdopcion.AdoptanteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "MascotaId", "MascotaId", registroAdopcion.MascotaId);
            return View(registroAdopcion);
        }

        // GET: RegistrosAdopcion/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroAdopcion = await _context.RegistrosAdopcion.FindAsync(id);
            if (registroAdopcion == null)
            {
                return NotFound();
            }
            ViewData["AdoptanteId"] = new SelectList(_context.Adoptante, "AdoptanteId", "AdoptanteId", registroAdopcion.AdoptanteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "MascotaId", "MascotaId", registroAdopcion.MascotaId);
            return View(registroAdopcion);
        }

        // POST: RegistrosAdopcion/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RegistroAdopcionId,MascotaId,AdoptanteId,FechaAdop,EnumProceso")] RegistroAdopcion registroAdopcion)
        {
            if (id != registroAdopcion.RegistroAdopcionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(registroAdopcion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RegistroAdopcionExists(registroAdopcion.RegistroAdopcionId))
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
            ViewData["AdoptanteId"] = new SelectList(_context.Adoptante, "AdoptanteId", "AdoptanteId", registroAdopcion.AdoptanteId);
            ViewData["MascotaId"] = new SelectList(_context.Mascotas, "MascotaId", "MascotaId", registroAdopcion.MascotaId);
            return View(registroAdopcion);
        }

        // GET: RegistrosAdopcion/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var registroAdopcion = await _context.RegistrosAdopcion
                .Include(r => r.Adoptante)
                .Include(r => r.Mascota)
                .FirstOrDefaultAsync(m => m.RegistroAdopcionId == id);
            if (registroAdopcion == null)
            {
                return NotFound();
            }

            return View(registroAdopcion);
        }

        // POST: RegistrosAdopcion/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroAdopcion = await _context.RegistrosAdopcion.FindAsync(id);
            _context.RegistrosAdopcion.Remove(registroAdopcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RegistroAdopcionExists(int id)
        {
            return _context.RegistrosAdopcion.Any(e => e.RegistroAdopcionId == id);
        }
    }
}
