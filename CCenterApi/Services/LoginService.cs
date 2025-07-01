using CCenterApi.Data;
using CCenterApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CCenterApi.Services
{
    public class LoginService
    {
        private readonly ApplicationDbContext _context;

        public LoginService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> UsuarioExiste(int userId)
        {
            return await _context.Users.AnyAsync(u => u.User_id == userId);
        }

        public async Task<bool> TieneLoginAbierto(int userId)
        {
            return await _context.Logins.AnyAsync(l =>
                l.User_id == userId &&
                l.TipoMov == 1 &&
                !_context.Logins.Any(x =>
                    x.User_id == userId &&
                    x.TipoMov == 0 &&
                    x.fecha > l.fecha));
        }
    }
}
