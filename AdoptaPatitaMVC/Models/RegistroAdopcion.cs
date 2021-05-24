using System;
using System.ComponentModel.DataAnnotations;

namespace AdoptaPatitaMVC.Models
{
    public class RegistroAdopcion{

        [Key]
        public int RegistroAdopcionId { get; set; }
        public int MascotaId { get; set; }
        public int AdoptanteId { get; set; }
        public Mascota Mascota{get;set;}
        public Adoptante Adoptante { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaAdop { get; set; }
        public EstadoProceso EnumProceso { get; set; }
    }
	
    public enum EstadoProceso{
        EN_PROCESO,
        ACEPTADO,
        RECHAZADO
    }
}