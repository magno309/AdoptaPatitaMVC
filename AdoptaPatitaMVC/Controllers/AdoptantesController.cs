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
    [AllowAnonymous]
    public class AdoptantesController : Controller
    {
        private readonly AdoptaPatitaContext _context;

        public AdoptantesController(AdoptaPatitaContext context)
        {
            _context = context;
        }

        // GET: Adoptantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Adoptante.ToListAsync());
        }

        // GET: Adoptantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptante = await _context.Adoptante
                .FirstOrDefaultAsync(m => m.AdoptanteId == id);
            if (adoptante == null)
            {
                return NotFound();
            }

            return View(adoptante);
        }

        // GET: Adoptantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Adoptantes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdoptanteId,Nombre,Apellido1,Apellido2,Telefono,Email,Calle,Colonia,Ciudad,Estado,FechaN")] Adoptante adoptante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(adoptante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(adoptante);
        }

        // GET: Adoptantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptante = await _context.Adoptante.FindAsync(id);
            if (adoptante == null)
            {
                return NotFound();
            }
            return View(adoptante);
        }

        // POST: Adoptantes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("AdoptanteId,Nombre,Apellido1,Apellido2,Telefono,Email,Calle,Colonia,Ciudad,Estado,FechaN")] Adoptante adoptante)
        {
            if (id != adoptante.AdoptanteId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(adoptante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AdoptanteExists(adoptante.AdoptanteId))
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
            return View(adoptante);
        }

        // GET: Adoptantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var adoptante = await _context.Adoptante
                .FirstOrDefaultAsync(m => m.AdoptanteId == id);
            if (adoptante == null)
            {
                return NotFound();
            }

            return View(adoptante);
        }

        // POST: Adoptantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var adoptante = await _context.Adoptante.FindAsync(id);
            _context.Adoptante.Remove(adoptante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AdoptanteExists(int id)
        {
            return _context.Adoptante.Any(e => e.AdoptanteId == id);
        }
    }
}
