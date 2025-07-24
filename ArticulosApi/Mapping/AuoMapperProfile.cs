using ArticulosApi.Dtos;
using ArticulosApi.Models;
using AutoMapper;

namespace ArticulosApi.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Articulo, ArticuloDto>()
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Usuario.NombreUsuario)).ForMember(dest => dest.AutorId, opt => opt.MapFrom(src => src.UsuarioId));

            CreateMap<Comentario, ComentarioDto>()
                .ForMember(dest => dest.Autor, opt => opt.MapFrom(src => src.Usuario==null ? "Anónimo": src.Usuario.NombreUsuario));
        }
    }
}
