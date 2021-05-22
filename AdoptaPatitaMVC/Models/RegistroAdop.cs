using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoptaPatitaMVC.Models
{
    public class RegistroAdop{
        public int MascotaId { get; set; }
        public int AdoptanteId { get; set; }
        public Mascota Mascota{get;set;}
        public Adoptante Adoptante { get; set; }
        public DateTime FechaAdop { get; set; }
        public EstadoProceso EnumProceso { get; set; }
    }
	
    public enum EstadoProceso{
        EN_PROCESO,
        ACEPTADO,
        RECHAZADO
    }
}