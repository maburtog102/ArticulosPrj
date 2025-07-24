using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArticulosApi.Models
{
    public class Articulo
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        [Required]
        public string Resumen { get; set; }

        [Required]
        public string Contenido { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario Usuario { get; set; }

        public List<Comentario> Comentarios { get; set; }
    }
}
