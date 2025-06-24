using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;

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
            var colecoes = _context.Colecoes
                .Include(c => c.ListaFotografias)
                .ToList();

            return View(colecoes);
        }

        // GET: Página de upload 
        [HttpGet]
        public IActionResult Upload()
        {
            // Lista de coleções
            ViewBag.ListaColecoes = _context.Colecoes.ToList();

            // Lista de fotografias e nova coleção para o formulário
            ViewBag.NovaFoto = new Fotografias();
            // var lista = _context.Fotografias.ToList();

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();

            var novaColecao = new Colecao();
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        // POST: Upload de nova fotografia
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(Fotografias novaFoto, IFormFile imagem)
        {
            if (ModelState.IsValid)
            {
                // Verifica se foi enviado um ficheiro
                if (imagem != null && imagem.Length > 0)
                {
                    // Gera um nome para o ficheiro, optamos por usar nomes aleatórios, pois assim serão unicos
                    var nomeFicheiro = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);

                    // Caminho da pasta onde o ficheiro é guardado
                    var caminho = Path.Combine("wwwroot/imagens", nomeFicheiro);

                    // Guarda o ficheiro 
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

            // Se houver erro de validação, volta a carregar tudo
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = novaFoto;

            var lista = _context.Fotografias
                .Include(f => f.Colecao)
                .ToList();

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
                // Caminho da imagem a eliminar
                var caminhoImagem = Path.Combine("wwwroot/imagens", foto.Ficheiro);

                // Verifica se o ficheiro existe e elimina-o
                if (System.IO.File.Exists(caminhoImagem))
                {
                    System.IO.File.Delete(caminhoImagem);
                }

                _context.Fotografias.Remove(foto);
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        //POST: Editar fotografia
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
                // O ficheiro da foto não é alteravel

                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        // POST: Criar coleção
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

            // Passar a lista de coleções para a ViewBag também no POST
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = new Fotografias();

            var lista = _context.Fotografias
                .Include(f => f.Colecao)
                .ToList();

            return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        // POST: Editar coleção
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarColecao(int Id, string Nome, string? Descricao)
        {
            // procura a coleção com o ID fornecido
            var colecao = _context.Colecoes.FirstOrDefault(c => c.Id == Id);

            if (colecao != null)
            {
                // atualiza os dados da coleção
                colecao.Nome = Nome;
                colecao.Descricao = Descricao;

                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        // POST: Eliminar coleção
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarColecao(int id)
        {
            
            var colecao = _context.Colecoes
                .Include(c => c.ListaFotografias)
                .FirstOrDefault(c => c.Id == id);

            if (colecao != null)
            {
                // Colecar as fotos da coleção "Sem coleção"
                foreach (var foto in colecao.ListaFotografias)
                {
                    foto.ColecaoFK = null;
                }

                // Eliminar a coleção
                _context.Colecoes.Remove(colecao);
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Loja()
        {
            var colecoes = _context.Colecoes
                .Include(c => c.ListaFotografias)
                .ToList();

            var fotosSemColecao = _context.Fotografias
                .Where(f => f.ColecaoFK == null)
                .ToList();

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
            if (foto == null)
            {
                return NotFound();
            }

            // carregar outras fotos da mesma coleção (ou aleatórias se ColecaoFK for null)
            var relacionadas = _context.Fotografias
                .Where(f => f.ColecaoFK == foto.ColecaoFK && f.Id != id)
                .Take(6)
                .ToList();

            ViewBag.Relacionadas = relacionadas;

            return View(foto);
        }
    }
}
