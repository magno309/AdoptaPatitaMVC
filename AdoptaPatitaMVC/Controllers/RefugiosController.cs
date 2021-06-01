using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Data;
using AdoptaPatitaMVC.Models;
using System.Text.Json;

namespace AdoptaPatitaMVC.Controllers
{
    public class RefugiosController : Controller
    {
        [BindProperty(Name ="idUsr")]
        public String IdUsr { get; set; }

        [BindProperty(Name ="codeUsr")]
        public String CodeUsr { get; set; }


        [BindProperty(Name ="urlUsr")]
        public String UrlUsr { get; set; }


        private readonly AdoptaPatitaContext _context;

        public RefugiosController(AdoptaPatitaContext context)
        {
            _context = context;
        }
        
        //GET: Refugios/Registro
        // PErmite a un usuario no logueado solicitar refistro.
        public async Task<IActionResult> RegistrarRef()
        {   
            return View();
        }

        public async Task<IActionResult> EnvioDatos( String IdUsr, String codeUsr, String urlUsr){

            Console.WriteLine("Aqui voy a guardar la info enviada: " + IdUsr + "  " + codeUsr + " " + urlUsr);
            if(TempData.ContainsKey("objRefugio")){
                String jsonStringRefugio  = TempData["objRefugio"].ToString();
                Console.WriteLine("JSON STRING: " + jsonStringRefugio);
                Refugio obj = JsonSerializer.Deserialize<Refugio>(jsonStringRefugio);
                Console.WriteLine(obj.Email + "  " + obj.Direccion);
                await Create(obj);
                Console.WriteLine("Llamé al create");
                var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = IdUsr, code = codeUsr, returnUrl = urlUsr },
                        protocol: Request.Scheme);
                Redirect(callbackUrl);
            } else {
                Console.WriteLine("No contiene valor el TEmpData");
                TempData["FalloCrearRefugio"] = "TRUE";
            }
            return View();
        }

        public async Task<IActionResult> SolicitudEnviada(){
            return View();
        }
        public async Task<IActionResult> ErrorSolicitud(){
            return View();
        }

        public async Task<IActionResult> ValidarEmail(string userId, string code, string  returnUrl){
            var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

            return View();
        }

        // GET: Refugios
        public async Task<IActionResult> Index()
        {
            return View(await _context.Refugios.ToListAsync());
        }

        // GET: Refugios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refugio = await _context.Refugios
                .FirstOrDefaultAsync(m => m.RefugioId == id);
            if (refugio == null)
            {
                return NotFound();
            }

            return View(refugio);
        }

        // GET: Refugios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Refugios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RefugioId,Nombre,Direccion,Telefono,Email,Contrasenia,Sitio_web")] Refugio refugio)
        {
            Console.WriteLine("Entramos a CREATE");
            if (ModelState.IsValid)
            {
                Console.WriteLine("Se está creando el registro ");
                _context.Add(refugio);
                await _context.SaveChangesAsync();
                Console.WriteLine("Se agregó al contexto");
                TempData["UsuarioCreado"] = "TRUE";

                return RedirectToAction(nameof(Index));
            }
            return View(refugio);
        }

        // GET: Refugios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refugio = await _context.Refugios.FindAsync(id);
            if (refugio == null)
            {
                return NotFound();
            }
            return View(refugio);
        }

        // POST: Refugios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RefugioId,Nombre,Direccion,Telefono,Email,Contrasenia,Sitio_web")] Refugio refugio)
        {
            if (id != refugio.RefugioId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(refugio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RefugioExists(refugio.RefugioId))
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
            return View(refugio);
        }

        // GET: Refugios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var refugio = await _context.Refugios
                .FirstOrDefaultAsync(m => m.RefugioId == id);
            if (refugio == null)
            {
                return NotFound();
            }

            return View(refugio);
        }

        // POST: Refugios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refugio = await _context.Refugios.FindAsync(id);
            _context.Refugios.Remove(refugio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RefugioExists(int id)
        {
            return _context.Refugios.Any(e => e.RefugioId == id);
        }
    }
}
