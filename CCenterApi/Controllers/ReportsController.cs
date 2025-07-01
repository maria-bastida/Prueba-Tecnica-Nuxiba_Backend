using CCenterApi.Data;
using CCenterApi.DTOs;
using CCenterApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CCenterApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReportsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("csv")]
        public async Task<IActionResult> GetCsvReport()
        {
            var users = await _context.Users
                .Include(u => u.Area)
                .Include(u => u.Logins)
                .ToListAsync();

            var report = new List<UserCsvReportDto>();

            foreach (var user in users)
            {
                var logins = user.Logins.OrderBy(l => l.fecha).ToList(); // Fecha corregida
                TimeSpan total = TimeSpan.Zero;

                for (int i = 0; i < logins.Count - 1; i++)
                {
                    if (logins[i].TipoMov == 1 && logins[i + 1].TipoMov == 0)
                    {
                        total += logins[i + 1].fecha - logins[i].fecha;
                        i++; // saltar el logout ya emparejado
                    }
                }

                report.Add(new UserCsvReportDto
                {
                    Login = user.Login,
                    NombreCompleto = $"{user.Nombres} {user.ApellidoPaterno} {user.ApellidoMaterno}".Trim(),
                    Area = user.Area?.AreaName ?? "Sin Área",
                    TotalHorasTrabajadas = Math.Round(total.TotalHours, 2)
                });
            }

            var csv = CsvGenerator.GenerateCsv(report);
return File(csv, "text/csv", "reporte.csv");
        }
    }
}
