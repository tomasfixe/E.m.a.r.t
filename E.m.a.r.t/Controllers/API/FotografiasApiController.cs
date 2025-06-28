using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;

namespace E.m.a.r.t.Controllers.API
{
    // Define a rota base da API: /api/FotografiasApi
    [Route("api/[controller]")]
    [ApiController]
    public class FotografiasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Injeta o contexto da base de dados
        public FotografiasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FotografiasApi
        // Devolve a lista de todas as fotografias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fotografias>>> GetFotografias()
        {
            return await _context.Fotografias.ToListAsync();
        }

        // GET: api/FotografiasApi/5
        // Devolve uma fotografia pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Fotografias>> GetFotografia(int id)
        {
            var fotografia = await _context.Fotografias.FindAsync(id);

            // Caso não encontre a fotografia, devolve Not Found
            if (fotografia == null)
                return NotFound(); 

            return fotografia;
        }

        // POST: api/FotografiasApi
        // Cria uma nova fotografia
        [HttpPost]
        public async Task<ActionResult<Fotografias>> PostFotografia(Fotografias foto)
        {
            _context.Fotografias.Add(foto);
            await _context.SaveChangesAsync();

            // Devolve resposta 201 com localização do novo recurso
            return CreatedAtAction(nameof(GetFotografia), new { id = foto.Id }, foto);
        }

        // PUT: api/FotografiasApi/5
        // Atualiza uma fotografia existente
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFotografia(int id, Fotografias foto)
        {
            if (id != foto.Id)
                return BadRequest(); // se os IDs não coincidem

            _context.Entry(foto).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent(); 
        }

        // DELETE: api/FotografiasApi/5
        // Elimina uma fotografia pelo ID
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
