using Microsoft.EntityFrameworkCore;
using AdoptaPatitaMVC.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AdoptaPatitaMVC.Data
{
    public class AdoptaPatitaContext : IdentityDbContext<IdentityUser>
    {
        public AdoptaPatitaContext(DbContextOptions<AdoptaPatitaContext> options) : base(options) {
        }



        public DbSet<Mascota> Mascotas { get; set; }
        public DbSet<Refugio> Refugios { get; set; }
        public DbSet<Administrador> Administradores { get; set; }
        public DbSet<Adoptante> Adoptante { get; set; }
        public DbSet<RegistroAdop> RegistroAdop { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
             builder.Entity<RegistroAdop>()
                .HasKey(r => new { r.MascotaId, r.AdoptanteId });
        }
    }
}
