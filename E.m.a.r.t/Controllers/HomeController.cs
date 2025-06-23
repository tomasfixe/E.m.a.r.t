using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace E.m.a.r.t.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            // Carrega todas as coleções com as fotografias incluídas
            var colecoes = _context.Colecoes
                .Include(c => c.Fotografias)
                .ToList();

            // Passa as coleções para a View
            return View(colecoes);
        }

        [HttpGet]
        public IActionResult Upload()
        {
            ViewBag.ListaColecoes = _context.Colecoes.ToList();

            ViewBag.NovaFoto = new Fotografias();

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();

            var novaColecao = new Colecao();
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(Fotografias novaFoto, IFormFile ficheiroUpload)
        {
            if (ModelState.IsValid)
            {
                if (ficheiroUpload != null && ficheiroUpload.Length > 0)
                {
                    var fileName = Path.GetFileName(ficheiroUpload.FileName);

                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ficheiroUpload.CopyToAsync(stream);
                    }

                    novaFoto.Ficheiro = fileName;

                    _context.Fotografias.Add(novaFoto);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Upload");
                }
                else
                {
                    ModelState.AddModelError("Ficheiro", "É obrigatório escolher uma fotografia.");
                }
            }

            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = novaFoto;

            var lista = _context.Fotografias
                .Include(f => f.Colecao)
                .ToList();

            var novaColecao = new Colecao();
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);

            if (foto != null)
            {
                _context.Fotografias.Remove(foto);
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, string Titulo, string? Descricao, DateTime Data, decimal Preco, int? ColecaoFK, string Ficheiro)
        {
            var foto = _context.Fotografias.FirstOrDefault(f => f.Id == Id);

            if (foto != null)
            {
                foto.Titulo = Titulo;
                foto.Descricao = Descricao;
                foto.Data = Data;
                foto.Preco = Preco;
                foto.ColecaoFK = ColecaoFK;
                // O ficheiro não é alterado ao editar

                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CriarColecao(Colecao novaColecao)
        {
            if (ModelState.IsValid)
            {
                _context.Colecoes.Add(novaColecao);
                _context.SaveChanges();
                return RedirectToAction("Upload");
            }

            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = new Fotografias();

            var lista = _context.Fotografias
                .Include(f => f.Colecao)
                .ToList();

            return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
