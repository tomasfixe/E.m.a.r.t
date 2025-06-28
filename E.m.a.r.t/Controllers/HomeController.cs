using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace E.m.a.r.t.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;  // Contexto do EF para aceder à base de dados
        private readonly ILogger<HomeController> _logger;  // Logger para registo de logs

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // --- CARRINHO DE COMPRAS ---

        [Authorize]  // Só permite acesso a utilizadores autenticados
        public IActionResult Checkout()
        {
            // Obtém o utilizador logado na base de dados
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);

            // Obtém a lista de fotografias do carrinho guardada na sessão (ou lista vazia)
            var fotosCarrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

            // Cria um ViewModel para passar para a View do carrinho
            var model = new CarrinhoViewModel
            {
                Nome = User.Identity?.Name ?? "Utilizador",
                NIF = utilizador?.NIF ?? "",
                Fotografias = fotosCarrinho
            };

            if (utilizador != null)
            {
                // Busca as compras do utilizador para mostrar no ViewBag
                var compras = _context.Compras
                    .Include(c => c.ListaFotografiasCompradas)
                    .Where(c => c.CompradorFK == utilizador.Id)
                    .ToList();

                ViewBag.Compras = compras;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]  // Requer login para finalizar compra
        public IActionResult FinalizarCompra(CarrinhoViewModel model, int? removerId)
        {
            // Obtém o carrinho da sessão
            var carrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

            if (removerId.HasValue)
            {
                // Se houver um id para remover, remove a foto do carrinho
                var fotoARemover = carrinho.FirstOrDefault(f => f.Id == removerId.Value);
                if (fotoARemover != null)
                {
                    carrinho.Remove(fotoARemover);
                    HttpContext.Session.SetObjectAsJson("Carrinho", carrinho); // Atualiza a sessão
                }

                // Retorna a View do carrinho atualizada
                return View("Checkout", new CarrinhoViewModel
                {
                    Nome = model.Nome,
                    NIF = model.NIF,
                    Fotografias = carrinho
                });
            }

            // Obter o utilizador da base de dados pelo User.Identity.Name
            var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);

            if (utilizador != null && carrinho.Any())
            {
                // Cria uma nova compra para guardar no BD
                var novaCompra = new Compras
                {
                    CompradorFK = utilizador.Id,
                    Data = DateTime.Now,
                    Estado = Estados.Pago,
                    ListaFotografiasCompradas = new List<Fotografias>()
                };

                // Para cada foto no carrinho, adiciona à lista da compra
                foreach (var foto in carrinho)
                {
                    var fotoDb = _context.Fotografias.Find(foto.Id);
                    if (fotoDb != null)
                    {
                        novaCompra.ListaFotografiasCompradas.Add(fotoDb);
                    }
                }

                _context.Compras.Add(novaCompra);  // Adiciona a compra ao contexto
                _context.SaveChanges();  // Salva as alterações no BD
            }

            HttpContext.Session.Remove("Carrinho"); // Limpa o carrinho da sessão

            // Carrega novamente as compras do utilizador para mostrar
            var comprasAtualizadas = _context.Compras
                .Include(c => c.ListaFotografiasCompradas)
                .Where(c => c.CompradorFK == utilizador.Id)
                .ToList();

            ViewBag.Compras = comprasAtualizadas;

            // Retorna a View do carrinho com lista vazia, pois a compra foi finalizada
            return View("Checkout", new CarrinhoViewModel
            {
                Nome = model.Nome,
                NIF = model.NIF,
                Fotografias = new List<Fotografias>() // carrinho está agora vazio
            });
        }

        [HttpPost]
        public IActionResult Adicionar(int id)
        {
            // Obtém o carrinho da sessão ou cria um novo
            var carrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

            if (!carrinho.Any(f => f.Id == id))  // Só adiciona se ainda não estiver no carrinho
            {
                var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
                if (foto != null)
                {
                    carrinho.Add(foto);
                    HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);  // Atualiza a sessão
                }
            }

            return RedirectToAction("Loja");  // Redireciona para a loja após adicionar
        }

        public IActionResult Confirmacao()
        {
            return View();  // View simples de confirmação (ex: compra finalizada)
        }

        // --- Upload / Galeria ---

        public IActionResult Index()
        {
            // Obtém coleções e suas fotografias para mostrar na página inicial
            var colecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
            return View(colecoes);
        }

        [Authorize(Roles = "Admin")] // Só utilizadores com a role Admin podem aceder
        [HttpGet]
        public IActionResult Upload()
        {
            ViewBag.ListaColecoes = _context.Colecoes.ToList();  // Lista para dropdown ou similar
            ViewBag.NovaFoto = new Fotografias();  // Objeto vazio para o formulário

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
            var novaColecao = new Colecao();

            // Passa uma tupla com lista de fotos e nova coleção para a View
            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(Fotografias novaFoto, IFormFile imagem)
        {
            if (ModelState.IsValid)
            {
                if (imagem != null && imagem.Length > 0)
                {
                    // Gera nome único para o ficheiro da imagem e guarda na pasta wwwroot/imagens
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
                return RedirectToAction("Upload");  // Redireciona para limpar formulário
            }

            // Se houver erro, recarrega dados para a View
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = novaFoto;

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
            var novaColecao = new Colecao();

            return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            // Apaga fotografia da base de dados e remove ficheiro do disco
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int Id, string Titulo, string? Descricao, DateTime Data, decimal Preco, int? ColecaoFK, string Ficheiro)
        {
            // Edita uma fotografia existente com os dados enviados
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

        [Authorize(Roles = "Admin")]
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

            // Em caso de erro, recarrega dados para a View
            ViewBag.ListaColecoes = _context.Colecoes.ToList();
            ViewBag.NovaFoto = new Fotografias();

            var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
            return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarColecao(int Id, string Nome, string? Descricao)
        {
            // Edita uma coleção existente com os dados enviados
            var colecao = _context.Colecoes.FirstOrDefault(c => c.Id == Id);

            if (colecao != null)
            {
                colecao.Nome = Nome;
                colecao.Descricao = Descricao;
                _context.SaveChanges();
            }

            return RedirectToAction("Upload");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EliminarColecao(int id)
        {
            // Apaga coleção e remove ligação das fotografias associadas
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
            // Obtém coleções e fotos sem coleção para mostrar na loja
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
            // Mostra detalhes de uma fotografia
            var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
            if (foto == null) return NotFound();

            // Busca fotografias relacionadas da mesma coleção (exclui a atual)
            var relacionadas = _context.Fotografias
                .Where(f => f.ColecaoFK == foto.ColecaoFK && f.Id != id)
                .Take(6)
                .ToList();

            ViewBag.Relacionadas = relacionadas;
            return View(foto);
        }

        public IActionResult Privacy()
        {
            return View();  // Página de política de privacidade
        }
    }

    // --- Extensões de Sessão (Json) ---
    public static class SessionExtensions
    {
        // Guarda um objeto na sessão serializado como JSON
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        // Obtém um objeto da sessão desserializando JSON
        public static T? GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
    }
}
