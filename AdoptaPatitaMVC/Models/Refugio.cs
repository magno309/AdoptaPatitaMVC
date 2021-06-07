using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptaPatitaMVC.Models
{
    [Table("Refugios")]
    public class Refugio
    {
        [Key]
        public int RefugioId { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        //[DataType(DataType.Upload)]
        //[Display(Name = "Imagen del refugio")]
        //[Required(ErrorMessage = "Por favor seleccione una imagen!")]
        //[NotMapped]
        //public IFormFile Imagen { get; set; }
        //public string ImagenURL { get; set; }
        public string Sitio_web { get; set; }

        public Refugio(int RefugioId,string Nombre,string Direccion,string Telefono,string Email,string Contrasenia,string Sitio_web){
            this.RefugioId = RefugioId; 
            this.Nombre = Nombre; 
            this.Direccion = Direccion; 
            this.Telefono = Telefono; 
            this.Email = Email; 
            this.Contrasenia = Contrasenia; 
            this.Sitio_web = Sitio_web; 
        }

        public Refugio(string Nombre,string Direccion,string Telefono,string Email,string Contrasenia,string Sitio_web){
            this.RefugioId = RefugioId; 
            this.Nombre = Nombre; 
            this.Direccion = Direccion; 
            this.Telefono = Telefono; 
            this.Email = Email; 
            this.Contrasenia = Contrasenia; 
            this.Sitio_web = Sitio_web; 
        }

        public Refugio(){
            
        }

    }
}
