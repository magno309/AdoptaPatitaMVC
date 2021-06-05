using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using AdoptaPatitaMVC.Models;
using System.Net;
using System.IO;
using Microsoft.AspNetCore.Http.Extensions; 
using AdoptaPatitaMVC.Controllers;
using System.Text.Json;
using System.Net.Http;

namespace AdoptaPatitaMVC.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager; // creado por mi
        

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender, 
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            Debug.WriteLine("Hola, se crea el modelo inicial");
            Console.WriteLine("Esto con consola");
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Correo elecrónico")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} y un máximo de {1} caracteres de longitud.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Contraseña")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirme su contraseña")]
            [Compare("Password", ErrorMessage = "Las contraseñas no coinciden.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            Console.WriteLine("GET ASYNC    returnUrl=" + returnUrl);
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        
        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            
            Debug.WriteLine("POST VALORES:  Email:" + Request.Form["Input.Email"]
            + "  Pass= " + Request.Form["Input.Password"]);
            if(Request.Form["Refugio"].Equals("TRUE")){
                Input.Email = Request.Form["Input.Email"];
                Input.Password = Request.Form["Input.Password"];
                Input.ConfirmPassword = Request.Form["Input.ConfirmPassword"];
            }
            
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid || Request.Form["Refugio"].Equals("TRUE"))
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);
                    // Para los refugios, no se les pide confirmación de Email, así que los datos se retornan para futura confirmación
                    if(Request.Form["Refugio"].Equals("TRUE")){                        
                        Refugio obj = new Refugio(Request.Form["Nombre"],
                                            Request.Form["Direccion"],Request.Form["Telefono"],Request.Form["Email"],
                                            Request.Form["Contrasenia"],Request.Form["Sitio_web"]);
                        
                        var options = new JsonSerializerOptions { WriteIndented = true };
                        string jsonString = JsonSerializer.Serialize(obj, options);

                        TempData["objRefugio"] = jsonString;
                        // Registrar el rol del usuario
                        if(_roleManager != null){
                            if (!await _roleManager.RoleExistsAsync(ConstRoles.RefugioRole))
                            {
                                await _roleManager.CreateAsync(new IdentityRole(ConstRoles.RefugioRole));
                            }
                            await _userManager.AddToRoleAsync(user, ConstRoles.RefugioRole);
                        } else {Console.WriteLine("RolMANAGER nulo");}

                        return RedirectToAction("EnvioDatos", "/Refugios", 
                                new{IdUsr = user.Id, codeUsr = code, urlUsr = returnUrl});
                    }
                    // Registrar el rol del usuario Adoptante
                    if(_roleManager != null){
                        if (!await _roleManager.RoleExistsAsync(ConstRoles.AdoptanteRole))
                        {
                            await _roleManager.CreateAsync(new IdentityRole(ConstRoles.AdoptanteRole));
                        }
                        await _userManager.AddToRoleAsync(user, ConstRoles.AdoptanteRole);
                    } else {Console.WriteLine("RolMANAGER nulo");}
                    // Añadir un registro 
                    var crearAdoptante = Url.Action("IntermediarioCreate","/Adoptantes", new{emailUsuario = Input.Email}, Request.Scheme);
                    HttpClient req = new HttpClient();
                    var content = await req.GetAsync(crearAdoptante);
                    Console.WriteLine(await content.Content.ReadAsStringAsync());

                    //
                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
