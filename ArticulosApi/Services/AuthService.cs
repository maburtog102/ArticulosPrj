using ArticulosApi.Data;
using ArticulosApi.Dtos;
using ArticulosApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ArticulosApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> RegisterAsync(RegistroDto dto)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == dto.Email))
                return false;

            /*Para demo creo un admin si el numero de usuarios es par, solo para tener de ambos usuarios*/
            int cant = _context.Usuarios.Count();

            CrearPasswordHash(dto.Password, out byte[] hash, out byte[] salt);

            var user = new Usuario
            {
                NombreUsuario = dto.NombreUsuario,
                Email = dto.Email,
                PasswordHash = hash,
                PasswordSalt = salt,
                Admin = cant % 2 == 0
            };

            _context.Usuarios.Add(user);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null || !VerificarPasswordHash(dto.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return CrearToken(user);
        }

        public async Task<UsuarioDto> GetUserAsync(string email)
        {
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return null;

            return new UsuarioDto
            {
                Id=user.Id,
                NombreUsuario = user.NombreUsuario,
                Email = user.Email,
                Admin = user.Admin
            };
        }

        private void CrearPasswordHash(string password, out byte[] hash, out byte[] salt)
        {
            using var hmac = new HMACSHA512();
            salt = hmac.Key;
            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private bool VerificarPasswordHash(string password, byte[] hash, byte[] salt)
        {
            using var hmac = new HMACSHA512(salt);
            var hashComparado = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return hashComparado.SequenceEqual(hash);
        }

        /// <summary>
        /// El tipo de claim Role es string, como estoy usando un boleano para el tipo de usuario debo cambiar entre admin o user segun la bandera Admin
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string CrearToken(Usuario user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Admin?"admin":"user")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}