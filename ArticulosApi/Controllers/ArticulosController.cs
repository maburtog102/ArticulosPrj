using ArticulosApi.Data;
using ArticulosApi.Dtos;
using ArticulosApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ArticulosApi.Controllers
{
    [ApiController]
    [Route("api/articles")]
    public class ArticulosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public ArticulosController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Articulos
                .Include(a => a.Usuario)
                .OrderByDescending(a => a.FechaCreacion);


            var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            var result = _mapper.Map<List<ArticuloDto>>(items).OrderByDescending(o=> o.FechaCreacion).ThenBy(o=> o.Autor).ToList(); 
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var articulo = await _context.Articulos
                .Include(a => a.Usuario)
                .Include(a => a.Comentarios.OrderByDescending(o=> o.Fecha)).ThenInclude(c => c.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (articulo == null) return NotFound();

            var dto = _mapper.Map<ArticuloDto>(articulo);
            var comentarios = _mapper.Map<List<ComentarioDto>>(articulo.Comentarios);

            return Ok(new { articulo = dto, comentarios });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(NuevoArticuloDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return Unauthorized();

            var articulo = new Articulo
            {
                Titulo = dto.Titulo,
                Resumen = dto.Resumen,
                Contenido = dto.Contenido,
                UsuarioId = user.Id
            };

            _context.Articulos.Add(articulo);
            await _context.SaveChangesAsync();
            return Ok(new { id = articulo.Id });
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, NuevoArticuloDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);

            if (articulo == null) return NotFound();
            if (articulo.UsuarioId != user.Id && !user.Admin) return Forbid();

            articulo.Titulo = dto.Titulo;
            articulo.Resumen = dto.Resumen;
            articulo.Contenido = dto.Contenido;

            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);

            if (articulo == null) return NotFound();
            if (articulo.UsuarioId != user.Id && !user.Admin) return Forbid();

            _context.Articulos.Remove(articulo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [Authorize]
        [HttpPost("{id}/comments")]
        public async Task<IActionResult> Comentar(int id, NuevoComentarioDto dto)
        {
            var email = User.FindFirstValue(ClaimTypes.Name);
            var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
            var articulo = await _context.Articulos.FirstOrDefaultAsync(a => a.Id == id);
            if (user == null || articulo == null) return BadRequest();

            var comentario = new Comentario
            {
                Texto = dto.Texto,
                UsuarioId = user.Id,
                ArticuloId = articulo.Id
            };

            _context.Comentarios.Add(comentario);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}