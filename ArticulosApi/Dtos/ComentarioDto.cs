namespace ArticulosApi.Dtos
{
    public class ComentarioDto
    {
        public int Id { get; set; }
        public string Texto { get; set; }
        public string Autor { get; set; }
        public DateTime Fecha { get; set; }
    }
}
