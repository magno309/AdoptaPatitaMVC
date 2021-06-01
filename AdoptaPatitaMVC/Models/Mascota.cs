using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AdoptaPatitaMVC.Models
{

    [Table("Mascotas")]
    public class Mascota
    {
        [Key]
        public int MascotaId { get; set; }
        public string Nombre { get; set; }
        public string Raza { get; set; }
        public string Color { get; set; }
        public string Sexo { get; set; }
        public string Edad { get; set; }
        public string Peso { get; set; }
        public string Tamanio { get; set; }
        public string Esterilizado { get; set; }
        public string Descripcion { get; set; }
        public string Historia { get; set; }
        public string Imagen1 { get; set; }
        public string Imagen2 { get; set; }
        public string Imagen3 { get; set; }
        public string Id_Refugio { get; set; }

    }
}
