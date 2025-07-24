using ArticulosApi.Dtos;

namespace ArticulosApi.Services
{
    
        public interface IAuthService
        {
            Task<bool> RegisterAsync(RegistroDto dto);
            Task<string> LoginAsync(LoginDto dto);
            Task<UsuarioDto> GetUserAsync(string email);
        }
    
}
