namespace ArticulosApi.Dtos
{
    public class ArticuloDto
    {
        public int Id { get; set; }
        public int AutorId { get; set; }
        public string Titulo { get; set; }
        public string Resumen { get; set; }
        public string Contenido { get; set; }
        public string Autor { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
