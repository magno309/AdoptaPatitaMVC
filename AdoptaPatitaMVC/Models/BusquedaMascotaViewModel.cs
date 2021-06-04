using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace AdoptaPatitaMVC.Models
{
    public class BusquedaMascotaViewModel
    {
        public List<Mascota> Mascotas { get; set; }
        public SelectList Tamanios { get; set; }
        public SelectList Sexos { get; set; }
        public SelectList Edades { get; set; }
        public string TamanioMascota { get; set; }
        public string SexoMascota { get; set; }
        public string EdadMascota { get; set; }

    }
}
