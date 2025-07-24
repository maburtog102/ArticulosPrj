using ArticulosApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ArticulosApi.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Articulo> Articulos { get; set; }
        public DbSet<Comentario> Comentarios { get; set; }

    }
}
