using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Controllers
{
    /// <summary>
    /// API para gestão de utilizadores.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UtilizadoresApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto da base de dados.
        /// </summary>
        public UtilizadoresApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve todos os utilizadores.
        /// </summary>
        /// <returns>Lista de utilizadores.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Utilizadores>>> GetUtilizadores()
        {
            return await _context.Utilizadores.ToListAsync();
        }

        /// <summary>
        /// Devolve um utilizador pelo seu ID.
        /// </summary>
        /// <param name="id">ID do utilizador.</param>
        /// <returns>Utilizador encontrado ou NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Utilizadores>> GetUtilizador(int id)
        {
            var utilizador = await _context.Utilizadores.FindAsync(id);

            if (utilizador == null)
                return NotFound();

            return utilizador;
        }

        /// <summary>
        /// Adiciona um novo utilizador à base de dados.
        /// </summary>
        /// <param name="utilizador">Objeto utilizador a adicionar.</param>
        /// <returns>Resposta 201 com o novo utilizador criado.</returns>
        [HttpPost]
        public async Task<ActionResult<Utilizadores>> PostUtilizador(Utilizadores utilizador)
        {
            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUtilizador), new { id = utilizador.Id }, utilizador);
        }

        /// <summary>
        /// Atualiza um utilizador existente.
        /// </summary>
        /// <param name="id">ID do utilizador a atualizar.</param>
        /// <param name="utilizador">Objeto utilizador atualizado.</param>
        /// <returns>NoContent se atualizado com sucesso, BadRequest se IDs diferentes.</returns>
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

        /// <summary>
        /// Elimina um utilizador pelo seu ID.
        /// </summary>
        /// <param name="id">ID do utilizador a eliminar.</param>
        /// <returns>NoContent se sucesso, NotFound se não encontrado.</returns>
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

        /// <summary>
        /// Verifica se um utilizador existe dado o ID.
        /// </summary>
        /// <param name="id">ID do utilizador.</param>
        /// <returns>True se existir, false caso contrário.</returns>
        private bool UtilizadorExists(int id)
        {
            return _context.Utilizadores.Any(u => u.Id == id);
        }
    }
}
