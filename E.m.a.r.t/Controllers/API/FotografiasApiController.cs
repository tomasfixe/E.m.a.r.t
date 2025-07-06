using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;

namespace E.m.a.r.t.Controllers.API
{
    /// <summary>
    /// Controller API para gerir fotografias.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FotografiasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor que recebe o contexto da base de dados.
        /// </summary>
        public FotografiasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve todas as fotografias da base de dados.
        /// </summary>
        /// <returns>Lista de fotografias.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fotografias>>> GetFotografias()
        {
            return await _context.Fotografias.ToListAsync();
        }

        /// <summary>
        /// Devolve uma fotografia específica pelo seu ID.
        /// </summary>
        /// <param name="id">ID da fotografia.</param>
        /// <returns>Fotografia encontrada ou NotFound.</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Fotografias>> GetFotografia(int id)
        {
            var fotografia = await _context.Fotografias.FindAsync(id);

            if (fotografia == null)
                return NotFound();

            return fotografia;
        }

        /// <summary>
        /// Cria uma nova fotografia na base de dados.
        /// </summary>
        /// <param name="foto">Objeto fotografia a criar.</param>
        /// <returns>Resposta 201 com a nova fotografia criada.</returns>
        [HttpPost]
        public async Task<ActionResult<Fotografias>> PostFotografia(Fotografias foto)
        {
            _context.Fotografias.Add(foto);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetFotografia), new { id = foto.Id }, foto);
        }

        /// <summary>
        /// Atualiza uma fotografia existente com base no ID.
        /// </summary>
        /// <param name="id">ID da fotografia a atualizar.</param>
        /// <param name="foto">Objeto fotografia atualizado.</param>
        /// <returns>NoContent se sucesso, BadRequest se IDs diferentes.</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFotografia(int id, Fotografias foto)
        {
            if (id != foto.Id)
                return BadRequest();

            _context.Entry(foto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Elimina uma fotografia pelo seu ID.
        /// </summary>
        /// <param name="id">ID da fotografia a eliminar.</param>
        /// <returns>NoContent se sucesso, NotFound se não encontrada.</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFotografia(int id)
        {
            var fotografia = await _context.Fotografias.FindAsync(id);
            if (fotografia == null)
                return NotFound();

            _context.Fotografias.Remove(fotografia);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
