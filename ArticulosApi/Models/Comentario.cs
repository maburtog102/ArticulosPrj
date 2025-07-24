using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ArticulosApi.Models
{
    public class Comentario
    {
        public int Id { get; set; }

        [Required]
        public string Texto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        public int UsuarioId { get; set; }
        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        public int ArticuloId { get; set; }
        [ForeignKey("ArticuloId")]
        public Articulo Articulo { get; set; }
    }
}
