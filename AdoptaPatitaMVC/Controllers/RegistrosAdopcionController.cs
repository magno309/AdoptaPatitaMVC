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
using Microsoft.AspNetCore.Identity;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize(Roles = "AdoptanteRole, RefugioRole, AdminRole")]
    public class RegistrosAdopcionController : Controller
    {
        private readonly AdoptaPatitaContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RegistrosAdopcionController(AdoptaPatitaContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: RegistrosAdopcion
        public async Task<ActionResult> Index(string Busqueda)
        {                        
            var emailUserAct = _userManager.GetUserName(User);

            if(User.IsInRole(ConstRoles.AdoptanteRole)){                                                
                var adoptante = await _context.Adoptante.FirstOrDefaultAsync(a => a.Email == emailUserAct);
                int adoptanteId = adoptante.AdoptanteId;
                /*var registrosAdop = _context.RegistrosAdopcion.Include(r => r.Adoptante).Include(r => r.Mascota)
                                .Where(r => r.AdoptanteId == adoptanteId);
                return View(await registrosAdop.ToListAsync());*/
                List<ListaRegistrosAdopcion> listaRegistros;
                if(!String.IsNullOrEmpty(Busqueda)){
                    listaRegistros = await (from r in _context.RegistrosAdopcion                             
                        join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                        join f in _context.Refugios on m.Id_Refugio equals f.RefugioId.ToString()   
                        where r.AdoptanteId == adoptanteId  
                        orderby r.FechaAdop descending                       
                        select new ListaRegistrosAdopcion{
                            ID = r.RegistroAdopcionId,
                            Mascota = m.Nombre,
                            Refugio = f.Nombre,
                            Adoptante = emailUserAct,
                            Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                            Estado = r.EnumProceso
                        }).Where(r => r.Mascota.Contains(Busqueda)).ToListAsync();
                    //Console.WriteLine(listaRegistros);
                    return View("IndexR", listaRegistros);
                }
                listaRegistros = await (from r in _context.RegistrosAdopcion                             
                    join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                    join f in _context.Refugios on m.Id_Refugio equals f.RefugioId.ToString()   
                    where r.AdoptanteId == adoptanteId  
                    orderby r.FechaAdop descending                       
                    select new ListaRegistrosAdopcion{
                        ID = r.RegistroAdopcionId,
                        Mascota = m.Nombre,
                        Refugio = f.Nombre,
                        Adoptante = emailUserAct,
                        Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                        Estado = r.EnumProceso
                    }).ToListAsync();
                //Console.WriteLine(listaRegistros);
                return View("IndexR", listaRegistros);

                //registros = registros.Where(r => r.AdoptanteId.ToString() == idUserAct);
                //var registrosAdop = _context.RegistrosAdopcion.Include(r => r.Mascota);
                //return View(await registrosAdop.ToListAsync());
            }
            if(User.IsInRole(ConstRoles.RefugioRole)){
                var refugio = await _context.Refugios.FirstOrDefaultAsync(r => r.Email == emailUserAct);
                string refugioId = refugio.RefugioId.ToString();                
                /*var registrosRefu = _context.RegistrosAdopcion.Include(r => r.Adoptante).Include(r => r.Mascota)
                                .Where(r => r.Mascota.Id_Refugio == refugioId)
                                .Where(r => r.EnumProceso == EstadoProceso.EN_PROCESO);
                return View(await registrosRefu.ToListAsync());*/
                List<ListaRegistrosAdopcion> listaRegistros;
                if(!String.IsNullOrEmpty(Busqueda)){
                    listaRegistros = await (from r in _context.RegistrosAdopcion 
                        join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                        join a in _context.Adoptante on r.AdoptanteId equals a.AdoptanteId
                        where m.Id_Refugio == refugioId
                        orderby r.FechaAdop descending
                        select new ListaRegistrosAdopcion{
                            ID = r.RegistroAdopcionId,
                            Mascota = m.Nombre,
                            Refugio = refugio.Nombre,
                            Adoptante = a.Email,
                            Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                            Estado = r.EnumProceso
                        }).Where(r => r.Mascota.Contains(Busqueda)).ToListAsync();                
                    //Console.WriteLine(listaRegistros);
                    return View("IndexR", listaRegistros);
                }
                listaRegistros = await (from r in _context.RegistrosAdopcion 
                    join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                    join a in _context.Adoptante on r.AdoptanteId equals a.AdoptanteId
                    where m.Id_Refugio == refugioId
                    orderby r.FechaAdop descending
                    select new ListaRegistrosAdopcion{
                        ID = r.RegistroAdopcionId,
                        Mascota = m.Nombre,
                        Refugio = refugio.Nombre,
                        Adoptante = a.Email,
                        Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                        Estado = r.EnumProceso
                    }).ToListAsync();                
                //Console.WriteLine(listaRegistros);
                return View("IndexR", listaRegistros);

                //registros = registros.Where(r => r.)
                //var registrosRefu = _context.RegistrosAdopcion.Include(r => r.Adoptante).Include(r => r.Mascota);
                //return View(await registrosRefu.ToListAsync());
            } 
            /*var registros = _context.RegistrosAdopcion.Include(r => r.Adoptante).Include(r => r.Mascota);            
            return View(await registros.ToListAsync());*/
            List<ListaRegistrosAdopcion> listaRegistrosA;
            if(!String.IsNullOrEmpty(Busqueda)){
                listaRegistrosA = await (from r in _context.RegistrosAdopcion 
                    join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                    join a in _context.Adoptante on r.AdoptanteId equals a.AdoptanteId
                    join f in _context.Refugios on m.Id_Refugio equals f.RefugioId.ToString()
                    orderby r.FechaAdop descending
                    select new ListaRegistrosAdopcion{
                        ID = r.RegistroAdopcionId,
                        Mascota = m.Nombre,
                        Refugio = f.Nombre,
                        Adoptante = a.Email,
                        Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                        Estado = r.EnumProceso
                    }).Where(r => r.Mascota.Contains(Busqueda)).ToListAsync();                
                //Console.WriteLine(listaRegistros);
                return View("IndexR", listaRegistrosA);
            }
            listaRegistrosA = await (from r in _context.RegistrosAdopcion 
                join m in _context.Mascotas on r.MascotaId equals m.MascotaId
                join a in _context.Adoptante on r.AdoptanteId equals a.AdoptanteId
                join f in _context.Refugios on m.Id_Refugio equals f.RefugioId.ToString()
                orderby r.FechaAdop descending
                select new ListaRegistrosAdopcion{
                    ID = r.RegistroAdopcionId,
                    Mascota = m.Nombre,
                    Refugio = f.Nombre,
                    Adoptante = a.Email,
                    Fecha_Solicitud = r.FechaAdop.Date.ToString(),
                    Estado = r.EnumProceso
                }).ToListAsync();                
            //Console.WriteLine(listaRegistros);
            return View("IndexR", listaRegistrosA);
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
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
        [Authorize(Roles = "AdminRole")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var registroAdopcion = await _context.RegistrosAdopcion.FindAsync(id);
            _context.RegistrosAdopcion.Remove(registroAdopcion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles ="RefugioRole")]
        public async Task<IActionResult> IntermediarioConfirmacion(int id){
            var registroAdopcion = await _context.RegistrosAdopcion.FindAsync(id);
            registroAdopcion.EnumProceso = EstadoProceso.ACEPTADO;
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

        [Authorize(Roles ="RefugioRole")]
        public async Task<IActionResult> IntermediarioRechazo(int id){
            var registroAdopcion = await _context.RegistrosAdopcion.FindAsync(id);
            registroAdopcion.EnumProceso = EstadoProceso.RECHAZADO;
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

        private bool RegistroAdopcionExists(int id)
        {
            return _context.RegistrosAdopcion.Any(e => e.RegistroAdopcionId == id);
        }

        public class ListaRegistrosAdopcion{
            public int ID {get; set;}
            public String Mascota {get; set;}
            public String Refugio {get; set;}
            public String Adoptante {get; set;}
            public String Fecha_Solicitud {get; set;}
            public EstadoProceso Estado {get; set;}
        }
    }
}
