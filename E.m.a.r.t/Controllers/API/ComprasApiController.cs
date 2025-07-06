using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ComprasApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Devolve todas as compras, incluindo dados do comprador e fotografias compradas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            return await _context.Compras
                .Include(c => c.Comprador)
                .Include(c => c.ListaFotografiasCompradas)
                .ToListAsync();
        }

        /// <summary>
        /// Devolve uma compra específica pelo ID, com dados completos
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Compras>> GetCompras(int id)
        {
            var compra = await _context.Compras
                .Include(c => c.Comprador)
                .Include(c => c.ListaFotografiasCompradas)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null)
                return NotFound();

            return compra;
        }

        /// <summary>
        /// Cria uma nova compra e guarda no banco de dados
        /// </summary>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras(Compras compra)
        {
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompras), new { id = compra.Id }, compra);
        }

        /// <summary>
        /// Atualiza uma compra existente com base no ID e dados fornecidos
        /// </summary>
        /// <param name="id"></param>
        /// <param name="compra"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCompras(int id, Compras compra)
        {
            if (id != compra.Id)
                return BadRequest();

            _context.Entry(compra).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        /// <summary>
        /// Apaga uma compra pelo ID, se existir
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompras(int id)
        {
            var compra = await _context.Compras.FindAsync(id);
            if (compra == null)
                return NotFound();

            _context.Compras.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// Verifica se uma compra existe com base no ID fornecido.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }
    }
}
