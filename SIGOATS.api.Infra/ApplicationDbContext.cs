using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Models;

namespace SIGOATS.api.Infra
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<Anexos> Anexos { get; set; }
        public DbSet<Archivos> Archivos { get; set; }
        public DbSet<Contactenos> Contactenos { get; set; }
        public DbSet<Extenciones> Extenciones { get; set; }
        public DbSet<Imagenes> Imagenes { get; set; }
        public DbSet<Noticias> Noticias { get; set; }
        public DbSet<NoticiasDetalles> NoticiasDetalles { get; set; }
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<Paginas> Paginas { get; set; }
        public DbSet<RedesSociales> RedesSociales { get; set; }
        public DbSet<TiposRedesSociales> TiposRedesSociales { get; set; }

    }
}
