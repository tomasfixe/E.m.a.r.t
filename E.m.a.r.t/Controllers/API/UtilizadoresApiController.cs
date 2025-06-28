using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UtilizadoresApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UtilizadoresApi
        // Devolve todos os utilizadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizadores>>> GetUtilizadores()
        {
            return await _context.Utilizadores.ToListAsync();
        }

        // GET: api/UtilizadoresApi/5
        // Devolve um utilizador específico pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizadores>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);

            if (utilizador == null)
                return NotFound();

            return utilizador;
        }

        // POST: api/UtilizadoresApi
        // Adiciona um novo utilizador
        [HttpPost]
        public async Task<ActionResult<Utilizadores>> PostUtilizador(Utilizadores utilizador)
        {
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.Id }, utilizador);
        }

        // PUT: api/UtilizadoresApi/5
        // Atualiza um utilizador existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, Utilizadores utilizador)
        {
            if (id != utilizador.Id)
                return BadRequest();

            _context.Entry(utilizador).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadorExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // DELETE: api/UtilizadoresApi/5
        // Elimina um utilizador
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizador(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
                return NotFound();

            _context.Utilizadores.Remove(utilizador);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Verifica se o utilizador existe
        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadores.Any(u => u.Id == id);
        }
    }
}
