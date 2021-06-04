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
using System.Text.Encodings.Web;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;

namespace AdoptaPatitaMVC.Controllers
{
    [Authorize]
    public class SolicitudRefugioController : Controller{
        private readonly AdoptaPatitaContext _context;

        public SolicitudRefugioController(AdoptaPatitaContext context)
        {
            _context = context;
        }

        // Lista todas las solicitudes.
        [Authorize(Roles ="AdminRole")]
        public ActionResult Index()
        {
            Console.WriteLine("Estamos en la lista solicitures");
            List<ListaSolicitudes> listaSolicitudes = (from s in _context.SolicitudRefugios join r in _context.Refugios 
                                    on s.RefugioId equals r.RefugioId 
                                    select new ListaSolicitudes{
                                        Id = s.SolicitudRefugioId, 
                                        Nombre = r.Nombre,
                                        Direccion = r.Direccion,
                                        Email = r.Email,
                                        Telefono = r.Telefono
                                    }).ToList();
            
            Console.WriteLine(listaSolicitudes);
            
            //ViewData.Model = listaSolicitudes;
            
            return View(listaSolicitudes.ToList());
        }
        

        
        [AllowAnonymous]
        public async Task<IActionResult> IntermediarioCreate(){
            Console.WriteLine("Intermediario: ");
            String jsonStringSolicitud  = TempData["solicitud"].ToString();
            Console.WriteLine("JSON STRING SOLICITUD: " + jsonStringSolicitud);
            SolicitudRefugio solicitud = JsonSerializer.Deserialize<SolicitudRefugio>(jsonStringSolicitud);
            
            return await Create(solicitud);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Create([Bind("SolicitudRefugioId,RefugioId,EsAceptado,userId,code,returnUrl")] SolicitudRefugio solicitud)
        {
            Console.WriteLine("Entramos a CREATE SOLICITUD REFUGIO");
            if (ModelState.IsValid)
            {
                Console.WriteLine("Se está creando el registro.");
                _context.Add(solicitud);
                await _context.SaveChangesAsync();
                Console.WriteLine("Se agregó al contexto");
                TempData["SolicitudRefugio"] = true;
                return RedirectToAction(nameof(Index));
            }
            TempData["SolicitudRefugio"] = false;
            return View();
        }

        
        [Authorize(Roles ="AdminRole")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var solicitud = await _context.SolicitudRefugios
                .FirstOrDefaultAsync(m => m.SolicitudRefugioId == id);
            if (solicitud == null)
            {
                return NotFound();
            }

            return View(solicitud);
        }

        // POST: SolicitudRefugio/Delete/5
        [Authorize(Roles ="AdminRole")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var solicitud = await _context.SolicitudRefugios.FindAsync(id);
            _context.SolicitudRefugios.Remove(solicitud);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        
        [Authorize(Roles ="AdminRole")]
        public async Task<IActionResult> ConfirmarEmail(int Id){
            try{
                var obj = await _context.SolicitudRefugios.FindAsync(Id);
                if(obj != null){     
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = obj.userId, code = obj.code, returnUrl = obj.returnUrl },
                        protocol: Request.Scheme);
                    // Debería confirmar que efectivamente se validó el email, pero no sé en qué punto hacerlo, así que acá va
                    await DeleteConfirmed(Id);
                    
                    HttpClient req = new HttpClient();
                    var content = await req.GetAsync(callbackUrl);
                    //HttpResponseMessage response = content.Res
                    Console.WriteLine(await content.Content.ReadAsStringAsync());

                    return Redirect(callbackUrl);
                }
            } catch(Exception ex){
                Console.WriteLine("Error al confirmar mail: " + ex.Message);
            }
            return View("../Refugios/ErrorSolicitud");
        }

        [Authorize(Roles ="AdminRole")]
        public async Task<IActionResult> IntermediarioConfirmacion(int Id){
            await ConfirmarEmail(Id);
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles ="AdminRole")]
        public async Task<IActionResult> IntermediarioRechazo(int Id){
            var obj = await _context.SolicitudRefugios.FindAsync(Id);
            var callbackUrl = Url.Action("intermediarioDelete","/Refugios", new{id = obj.RefugioId}, Request.Scheme);

            HttpClient req = new HttpClient();
            var content = await req.GetAsync(callbackUrl);
            //HttpResponseMessage response = content.Res
            Console.WriteLine(await content.Content.ReadAsStringAsync());

            await DeleteConfirmed(Id);
            
            return RedirectToAction(nameof(Index));
        }

    }

        public class ListaSolicitudes{
            public int Id {get; set;}
            public String Nombre {get; set;}
            public String Direccion {get; set;}
            public String Email {get; set;}
            public String Telefono {get; set;}
        }

}