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
    }
}
