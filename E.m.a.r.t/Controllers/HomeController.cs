using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;  // para [Authorize]

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

        // --- CARRINHO DE COMPRAS ---

        [Authorize]  // Só permite acesso a utilizadores autenticados
        public IActionResult Checkout()
        {
            var model = new CarrinhoViewModel
            {
                Nome = User.Identity?.Name ?? "Utilizador",
                Morada = "Morada pré-preenchida",
                NIF = "123456789",
                Fotografias = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Requer login para finalizar compra
        public IActionResult FinalizarCompra(CarrinhoViewModel model, int? removerId)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

            if (removerId.HasValue)
            {
                var fotoARemover = carrinho.FirstOrDefault(f => f.Id == removerId.Value);
                if (fotoARemover != null)
                {
                    carrinho.Remove(fotoARemover);
                    HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
                }

                return View("Checkout", new CarrinhoViewModel
                {
                    Nome = model.Nome,
                    NIF = model.NIF,
                    Morada = model.Morada,
                    Fotografias = carrinho
                });
            }

            // Aqui podes guardar o pedido na base de dados, enviar email, etc.

            HttpContext.Session.Remove("Carrinho"); // Limpa carrinho após finalização

            return RedirectToAction("Confirmacao");
        }

        [HttpPost]
        public IActionResult Adicionar(int id)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

            if (!carrinho.Any(f => f.Id == id))
            {
                var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
                if (foto != null)
                {
                    carrinho.Add(foto);
                    HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
                }
            }

            return RedirectToAction("Loja");
        }

        public IActionResult Confirmacao()
        {
            return View();
        }

        // --- Upload / Galeria ---

        public IActionResult Index()
        {
            var colecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
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
        public IActionResult Upload(Fotografias novaFoto, IFormFile imagem)
        {
            if (ModelState.IsValid)
            {
                if (imagem != null && imagem.Length > 0)
                {
                    var nomeFicheiro = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
                    var caminho = Path.Combine("wwwroot/imagens", nomeFicheiro);

                    using (var stream = new FileStream(caminho, FileMode.Create))
                    {
                        imagem.CopyTo(stream);
                    }

                    novaFoto.Ficheiro = nomeFicheiro;
                }

                _context.Fotografias.Add(novaFoto);
                _context.SaveChanges();
                return RedirectToAction("Upload");
            }

            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = novaFoto;

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
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
                var caminhoImagem = Path.Combine("wwwroot/imagens", foto.Ficheiro);
                if (System.IO.File.Exists(caminhoImagem))
                    System.IO.File.Delete(caminhoImagem);

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

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
            return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarColecao(int Id, string Nome, string? Descricao)
        {
            var colecao = _context.Colecoes.FirstOrDefault(c => c.Id == Id);

            if (colecao != null)
            {
                colecao.Nome = Nome;
                colecao.Descricao = Descricao;
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarColecao(int id)
        {
            var colecao = _context.Colecoes.Include(c => c.ListaFotografias).FirstOrDefault(c => c.Id == id);

            if (colecao != null)
            {
                foreach (var foto in colecao.ListaFotografias)
                    foto.ColecaoFK = null;

                _context.Colecoes.Remove(colecao);
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        public IActionResult Loja()
        {
            var colecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
            var fotosSemColecao = _context.Fotografias.Where(f => f.ColecaoFK == null).ToList();

            var vm = new LojaViewModel
            {
                Colecoes = colecoes,
                FotografiasSemColecao = fotosSemColecao
            };

            return View(vm);
        }

        public IActionResult Details(int id)
        {
            var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
            if (foto == null) return NotFound();

            var relacionadas = _context.Fotografias
                .Where(f => f.ColecaoFK == foto.ColecaoFK && f.Id != id)
                .Take(6)
                .ToList();

            ViewBag.Relacionadas = relacionadas;
            return View(foto);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }

    // --- Extensões de Sessão (Json)
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
