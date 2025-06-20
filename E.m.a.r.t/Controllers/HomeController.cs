using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

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
            return View();
        }

        // GET: P�gina de upload 
        [HttpGet]
        public IActionResult Upload()
        {
            // Lista de cole��es
            ViewBag.ListaColecoes = _context.Colecoes.ToList();

            // Lista de fotografias e nova cole��o para o formul�rio
            ViewBag.NovaFoto = new Fotografias();
            var lista = _context.Fotografias.ToList();
            var novaColecao = new Colecao();
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        // POST: Upload de nova fotografia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(Fotografias novaFoto)
        {
            if (ModelState.IsValid)
            {
                _context.Fotografias.Add(novaFoto);
                _context.SaveChanges();
                return RedirectToAction("Upload");
            }

            // Passar a lista de cole��es para a ViewBag
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = novaFoto;
            var lista = _context.Fotografias.ToList();
            var novaColecao = new Colecao();
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        // POST: Eliminar fotografia
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

            return RedirectToAction(nameof(Index));
        }

        // POST: Criar cole��o
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

            // Passar a lista de cole��es para a ViewBag tamb�m no POST
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = new Fotografias();
            var lista = _context.Fotografias.ToList();
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

