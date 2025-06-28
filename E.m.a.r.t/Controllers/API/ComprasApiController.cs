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

        // GET: api/ComprasApi
        // Devolve a lista de todas as compras (com os dados do comprador)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Compras>>> GetCompras()
        {
            return await _context.Compras
                .Include(c => c.Comprador)
                .Include(c => c.ListaFotografiasCompradas)
                .ToListAsync();
        }

        // GET: api/ComprasApi/5
        // Devolve uma compra específica por ID
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

        // POST: api/ComprasApi
        // Cria uma nova compra
        [HttpPost]
        public async Task<ActionResult<Compras>> PostCompras(Compras compra)
        {
            _context.Compras.Add(compra);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompras), new { id = compra.Id }, compra);
        }

        // PUT: api/ComprasApi/5
        // Atualiza uma compra existente
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

        // DELETE: api/ComprasApi/5
        // Apaga uma compra existente
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

        // Verifica se uma compra existe por ID
        private bool CompraExists(int id)
        {
            return _context.Compras.Any(e => e.Id == id);
        }
    }
}
