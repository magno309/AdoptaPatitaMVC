using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoptaPatitaMVC.Models
{
    public class RegistroAdop{
        public int Id_Mascota { get; set; }
        public int Id_Adop { get; set; }
        public DateTime FechaAdop { get; set; }
    }
}