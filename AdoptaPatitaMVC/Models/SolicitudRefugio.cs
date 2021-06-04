using System;
using System.ComponentModel.DataAnnotations;

namespace AdoptaPatitaMVC.Models
{
    public class SolicitudRefugio{
        public int SolicitudRefugioId { get; set; }
        public int RefugioId { get; set; }
        public bool EsAceptado { get; set; }
        public String userId { get; set; }
        public String code { get; set; }
        public String returnUrl { get; set; }

        public SolicitudRefugio(){}
    }
}