﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptaPatitaMVC.Models
{
    [Table("Refugios")]
    public class Refugio
    {
        public int Id_Refugio { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Contrasenia { get; set; }
        public string Sitio_web { get; set; }

    }
}
