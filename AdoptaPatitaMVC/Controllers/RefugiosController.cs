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
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace AdoptaPatitaMVC.Controllers
{
    [AllowAnonymous]
    public class RefugiosController : Controller
    {
        [BindProperty(Name ="idUsr")]
        public String IdUsr { get; set; }

        [BindProperty(Name ="codeUsr")]
        public String CodeUsr { get; set; }


        [BindProperty(Name ="urlUsr")]
        public String UrlUsr { get; set; }

        SolicitudRefugio solicitud;
        private readonly AdoptaPatitaContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public RefugiosController(AdoptaPatitaContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }
        
        //GET: Refugios/Registro
        // PErmite a un usuario no logueado solicitar refistro.
        [AllowAnonymous]
        public async Task<IActionResult> RegistrarRef()
        {   
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> EnvioDatos( String IdUsr, String codeUsr, String urlUsr){

            Console.WriteLine("Aqui voy a guardar la info enviada: " + IdUsr + "  " + codeUsr + " " + urlUsr);
            this.IdUsr = IdUsr;
            this.CodeUsr = codeUsr;
            this.UrlUsr = urlUsr;
            if(TempData.ContainsKey("objRefugio")){
                String jsonStringRefugio  = TempData["objRefugio"].ToString();
                Console.WriteLine("JSON STRING: " + jsonStringRefugio);
                Refugio obj = JsonSerializer.Deserialize<Refugio>(jsonStringRefugio);
                Console.WriteLine(obj.Email + "  " + obj.Direccion);
                await Create(obj);
                Console.WriteLine("Llamé al create");
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(this.solicitud, options);
                TempData["solicitud"] = jsonString;
                Console.WriteLine("Se guardo el json");
                
                return RedirectToAction("IntermediarioCreate", "/SolicitudRefugio");
                //Console.WriteLine("Se redirecciono y ya se regreso de ahí");                               
            } else {
                Console.WriteLine("No contiene valor el TEmpData");
                TempData["FalloCrearRefugio"] = "TRUE";
            }
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> SolicitudEnviada(){
            return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> ErrorSolicitud(){
            return View();
        }

        /*public async Task<IActionResult> ValidarEmail(string userId, string code, string  returnUrl){
            var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

            return View();
        }*/

        // GET: Refugios
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Refugios.ToListAsync());
            
        }

        // GET: Refugios/Details/5
        [AllowAnonymous]
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
        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Refugios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("RefugioId,Nombre,Direccion,Telefono,Email,Contrasenia,Sitio_web")] Refugio refugio)
        {
            Console.WriteLine("Entramos a CREATE");
            if (ModelState.IsValid)
            {
                if (refugio.Imagen != null)
                {
                    string folder = "imgRefugios\\";
                    string guid = Guid.NewGuid().ToString() + "_" + refugio.Imagen.FileName;
                    folder += guid;
                    string serverFolder = Path.Combine(_webHostEnvironment.WebRootPath, folder);
                    await refugio.Imagen.CopyToAsync(new FileStream(serverFolder, FileMode.Create));
                    refugio.ImagenURL = guid;
                }
                Console.WriteLine("Se está creando el registro ");
                _context.Add(refugio);
                await _context.SaveChangesAsync();
                Console.WriteLine("Se agregó al contexto");
                TempData["UsuarioCreado"] = "TRUE";
                this.solicitud = new SolicitudRefugio();
                this.solicitud.RefugioId = refugio.RefugioId;
                this.solicitud.userId = this.IdUsr;
                this.solicitud.code = this.CodeUsr;
                this.solicitud.returnUrl = this.UrlUsr;
                this.solicitud.EsAceptado = false;
                Console.WriteLine(" Solicitud refugio ID" + this.solicitud.RefugioId);
                
               return RedirectToAction(nameof(Index));
            }
            return View(refugio);
        }

        
        [Authorize(Roles ="AdminRole")]
        public async Task<IActionResult> intermediarioDelete(int id){
            Console.WriteLine("Entramos a Borrar REFUGIO");
            await DeleteConfirmed(id);
            return RedirectToAction(nameof(Index));
        }

        // GET: Refugios/Edit/5
        [Authorize(Roles ="AdminRole, RefugioRole")]
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
        [Authorize(Roles ="AdminRole, RefugioRole")]

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
        [Authorize(Roles ="AdminRole, RefugioRole")]

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
        [Authorize(Roles ="AdminRole, RefugioRole")]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var refugio = await _context.Refugios.FindAsync(id);
            _context.Refugios.Remove(refugio);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AllowAnonymous]
        private bool RefugioExists(int id)
        {
            return _context.Refugios.Any(e => e.RefugioId == id);
        }
    }
}
