using System.ComponentModel.DataAnnotations;

namespace ArticulosApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        public string NombreUsuario { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public byte[] PasswordHash { get; set; }

        [Required]
        public byte[] PasswordSalt { get; set; }

        public bool Admin { get; set; } = false; // manejaré solo dos tipos asi que puedo usar un boleano 
    }
}
