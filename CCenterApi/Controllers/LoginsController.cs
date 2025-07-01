using CCenterApi.Data;
using CCenterApi.Models;
using CCenterApi.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LoginsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoginDto>>> GetLogins()
        {
            var logins = await _context.Logins
                .Include(l => l.User)
                .Select(l => new LoginDto
                {
                    Id = l.Id,
                    User_id = l.User_id,
                    UserLogin = l.User.Login,
                    Extension = l.Extension,
                    TipoMov = l.TipoMov,
                    Fecha = l.fecha
                })
                .ToListAsync();

            return Ok(logins);
        }

        [HttpPost]
        public async Task<ActionResult<Login>> PostLogin(LoginCreateDto loginDto)
        {
            var userExists = await _context.Users.AnyAsync(u => u.User_id == loginDto.User_id);
            if (!userExists)
                return BadRequest("El usuario no existe");

            if (loginDto.TipoMov == 1)
            {
                var tieneLoginActivo = await _context.Logins.AnyAsync(l =>
                    l.User_id == loginDto.User_id &&
                    l.TipoMov == 1 &&
                    !_context.Logins.Any(lo =>
                        lo.User_id == loginDto.User_id &&
                        lo.TipoMov == 0 &&
                        lo.fecha > l.fecha));
                if (tieneLoginActivo)
                {
                    return BadRequest("El usuario ya tiene un login sin logout.");
                }
            }

            var login = new Login
            {
                User_id = loginDto.User_id,
                Extension = loginDto.Extension,
                TipoMov = loginDto.TipoMov,
                fecha = loginDto.Fecha
            };

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetLogins), new { id = login.Id }, login);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> PutLogin(int id, LoginUpdateDto loginDto)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login == null)
                return BadRequest("El usuario no existe");

            login.Extension = loginDto.Extension;
            login.TipoMov = loginDto.TipoMov;
            login.fecha = loginDto.Fecha;

            await _context.SaveChangesAsync();
            return Ok(new { message = "Login actualizado correctamente." });

        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLogin(int id)
        {
            var login = await _context.Logins.FindAsync(id);
            if (login == null)
                return BadRequest("El usuario no existe");

            _context.Logins.Remove(login);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Login eliminado correctamente." });
        }
    }
}
