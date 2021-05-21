using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Models;

namespace AdoptaPatitaMVC.Data
{
    public class AdoptaPatitaContext : DbContext
    {
        public AdoptaPatitaContext(DbContextOptions<AdoptaPatitaContext> options) : base(options) {
        }

        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Refugio> Refugios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
    }
}
