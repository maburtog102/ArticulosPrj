namespace ArticulosApi.Dtos
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public bool Admin { get; set; }
    }
}
