using System;
using System.ComponentModel.DataAnnotations;

namespace AdoptaPatitaMVC.Models
{
    public class Mensaje{
        public int MensajeId { get; set; }
        public int UserId { get; set; }
        public int RefugioId{ get; set; }
        public DateTime Fecha1 { get; set; }
        public DateTime Fecha2 { get; set; }
        public String Mensaje1 { get; set; }
        public String Mensaje2 { get; set; }
    }
}