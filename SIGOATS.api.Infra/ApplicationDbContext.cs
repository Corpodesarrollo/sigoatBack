using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SIGOATS.api.Core.Models;

namespace SIGOATS.api.Infra
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<Usuarios, Roles, long>(options)
    {
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityUserRole<long>>(entity =>
            {
                entity.ToTable("UserProfiles");

                entity.HasKey(up => new { up.UserId, up.RoleId });
            });

            builder.Entity<Usuarios>(entity =>
            {
                entity.ToTable("Usuarios");
            });

            builder.Entity<Roles>(entity =>
            {
                entity.ToTable("Roles");
            });

            // Configurar las tablas de Identity para usar long como tipo de clave primaria 
            builder.Entity<IdentityUserClaim<long>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<long>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<long>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });

            builder.Entity<IdentityUserToken<long>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
        }

        public DbSet<Configuracion> Configuracion { get; set; }
        public DbSet<Permisos> Permisos { get; set; }
        public DbSet<Menus> Menus { get; set; }
        public DbSet<Modulos> Modulos { get; set; }
        public DbSet<Usuarios> Usuarios { get; set; }
        public DbSet<Anexos> Anexos { get; set; }
        public DbSet<Archivos> Archivos { get; set; }
        public DbSet<Contactenos> Contactenos { get; set; }
        public DbSet<Extenciones> Extenciones { get; set; }
        public DbSet<Imagenes> Imagenes { get; set; }
        public DbSet<Noticias> Noticias { get; set; }
        public DbSet<NoticiasDetalles> NoticiasDetalles { get; set; }
        public DbSet<Notificaciones> Notificaciones { get; set; }
        public DbSet<NotificacionesLeidas> NotificacionesLeidas { get; set; }
        public DbSet<Paginas> Paginas { get; set; }
        public DbSet<RedesSociales> RedesSociales { get; set; }
        public DbSet<TiposRedesSociales> TiposRedesSociales { get; set; }
        public DbSet<MenusPortal> MenusPortal { get; set; }
        public DbSet<Tableros> Tableros { get; set; }
        public DbSet<EnlacesInteres> EnlacesInteres { get; set; }
        public DbSet<FooterInformacionInstitucional> FooterInformacionInstitucional { get; set; }
        public DbSet<FooterNormatividad> FooterNormatividad { get; set; }
        public DbSet<FooterFaq> FooterFaq { get; set; }
        public DbSet<ConfigSMTP> ConfigSMTP { get; set; }
        public DbSet<Emails> Emails { get; set; }
        public DbSet<CodigosSeguridad> CodigosSeguridad { get; set; }
        public DbSet<Variables> Variables { get; set; }
        public DbSet<EmailsVariables> EmailsVariables { get; set; }
    }
}
