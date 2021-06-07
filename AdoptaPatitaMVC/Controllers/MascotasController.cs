using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Data;
using AdoptaPatitaMVC.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using System;
using Microsoft.AspNetCore.Identity;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize(Roles = "RefugioRole, AdminRole")]
    public class MascotasController : Controller
    {
        private readonly AdoptaPatitaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public MascotasController(AdoptaPatitaContext context, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
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
                           join n in _context.RegistrosAdopcion
                           on m.MascotaId equals n.MascotaId into registroMascotas
                           from rm in registroMascotas.DefaultIfEmpty() where rm.EnumProceso != EstadoProceso.ACEPTADO
                           select new Mascota
                           {
                               MascotaId = m.MascotaId,
                               Nombre = m.Nombre,
                               Raza = m.Raza,
                               Color = m.Color,
                               Sexo = m.Sexo,
                               Edad = m.Edad,
                               Peso = m.Peso,
                               Tamanio = m.Tamanio,
                               Esterilizado = m.Esterilizado,
                               Descripcion = m.Descripcion,
                               Historia = m.Historia,
                               ImagenURL = m.ImagenURL,
                               Id_Refugio = m.Id_Refugio
                           };

            /*var mascotas = from m in _context.Mascotas
                         select m;

            var registros = from m in _context.RegistrosAdopcion
                         select m;*/

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

            if(User.IsInRole(ConstRoles.RefugioRole)){
                var emailUserAct = _userManager.GetUserName(User);
                int refugioId = (await _context.Refugios.FirstOrDefaultAsync(r => r.Email == emailUserAct)).RefugioId;
                mascotas = mascotas.Where(m => m.Id_Refugio == refugioId.ToString());
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

        [Authorize(Roles ="AdoptanteRole")]
        public async Task<IActionResult> SolicitudEnviada(){
            return View();
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
            if(User.IsInRole(ConstRoles.AdoptanteRole)){
                List<Adoptante> adoptante = _context.Adoptante.Where(a => a.Email.Equals(User.Identity.Name)).ToList();
                
                TempData["emailAdop"] = adoptante[0].Email;
                TempData["IdAdop"] = adoptante[0].AdoptanteId;

            }
            
            return View(mascota);
        }

        // GET: Mascotas/Create
        //[Authorize(Roles = "RefugioRole")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Mascotas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]        
        public async Task<IActionResult> Create([Bind("MascotaId,Nombre,Raza,Color,Sexo,Edad,Peso,Tamanio,Esterilizado,Descripcion,Historia,Imagen1,Id_Refugio")] Mascota mascota)
        {
            if (ModelState.IsValid)
            {
                if (mascota.Imagen1 != null) {
                    string folder = "imgMascotas\\";
                    string guid = Guid.NewGuid().ToString() + "_" + mascota.Imagen1.FileName;
                    folder += guid;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await mascota.Imagen1.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    mascota.ImagenURL = guid;
                }
                if(User.IsInRole(ConstRoles.RefugioRole)){
                    var emailUserAct = _userManager.GetUserName(User);
                    int refugioId = (await _context.Refugios.FirstOrDefaultAsync(r => r.Email == emailUserAct)).RefugioId;
                    mascota.Id_Refugio = refugioId.ToString();
                }                
                _context.Add(mascota);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(mascota);
        }
        
        // GET: Mascotas/Edit/5
        // [Authorize(Roles = "RefugioRole")]
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
        //[Authorize(Roles = "RefugioRole")]
        public async Task<IActionResult> Edit(int id, [Bind("MascotaId,Nombre,Raza,Color,Sexo,Edad,Peso,Tamanio,Esterilizado,Descripcion,Historia,Imagen1,Id_Refugio")] Mascota mascota)
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
        //[Authorize(Roles = "RefugioRole")]
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
        //[Authorize(Roles = "RefugioRole")]
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
