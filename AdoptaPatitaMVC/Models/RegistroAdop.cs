using System;
using System.ComponentModel.DataAnnotations;


namespace AdoptaPatitaMVC.Models
{
    public class RegistroAdop{
        public int Id_Mascota { get; set; }
        public int Id_Adop { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaAdop { get; set; }
    }
}