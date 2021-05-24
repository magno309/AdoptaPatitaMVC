using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptaPatitaMVC.Models
{
    [Table("Administradores")]
    public class Administrador
    {
        [Key]
        public int Id { get; set; }
        public string Usuario { get; set; }
        public string Contrasenia { get; set; }
    }
}
