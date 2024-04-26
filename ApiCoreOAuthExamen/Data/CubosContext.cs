using ApiCoreOAuthExamen.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCoreOAuthExamen.Data
{
    public class CubosContext: DbContext
    {
        public CubosContext(DbContextOptions<CubosContext> options)
            :base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cubo> Cubos { get; set; }
        public DbSet<PedidoCubo> Pedidos { get; set; }
    }
}
