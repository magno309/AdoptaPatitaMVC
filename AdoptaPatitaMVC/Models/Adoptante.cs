using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdoptaPatitaMVC.Models
{
    public class Adoptante{
        public int Id_Adop {get; set;}
        public String Nombre { get; set; }
        public String Apellido1 { get; set; }
        public String Apellido2 { get; set; }
        public String Telefono { get; set; }
        public String Email { get; set; }
        public String Calle { get; set; }
        public String Colonia { get; set; }
        public String Ciudad { get; set; }
        public String Estado { get; set; }
        public DateTime FechaN { get; set; }

        
    }
}