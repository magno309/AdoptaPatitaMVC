using System;
using System.ComponentModel.DataAnnotations;

namespace AdoptaPatitaMVC.Models
{
    public class SolicitudRefugio{
        public int SolicitudId { get; set; }
        public int RefugioId { get; set; }
        public bool EsAceptado { get; set; }
    }
}