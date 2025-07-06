using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using Emart.Helpers;

public class CarrinhoController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarrinhoController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Página de checkout do carrinho de compras.
    /// </summary>
    /// <returns></returns>
    [Authorize]
    public IActionResult Checkout()
    {
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);
        var fotosCarrinho = HttpContext.Session.GetObjectFromJson<List<Fotografias>>("Carrinho") ?? new List<Fotografias>();

        var model = new CarrinhoViewModel
        {
            Nome = User.Identity?.Name ?? "Utilizador",
            NIF = utilizador?.NIF ?? "",
            Fotografias = fotosCarrinho
        };

        if (utilizador != null)
        {
            var compras = _context.Compras
                .Include(c => c.ListaFotografiasCompradas)
                .Where(c => c.CompradorFK == utilizador.Id)
                .ToList();

            ViewBag.Compras = compras;
        }

        return View(model);
    }

    /// <summary>
    /// Finaliza a compra do carrinho, removendo fotografias selecionadas ou finalizando a compra.
    /// </summary>
    /// <param name="model"></param>
    /// <param name="removerId"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
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
                Fotografias = carrinho
            });
        }

        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);

        if (utilizador != null && carrinho.Any())
        {
            var novaCompra = new Compras
            {
                CompradorFK = utilizador.Id,
                Data = DateTime.Now,
                Estado = Estados.Pago,
                ListaFotografiasCompradas = new List<Fotografias>()
            };

            foreach (var foto in carrinho)
            {
                var fotoDb = _context.Fotografias.Find(foto.Id);
                if (fotoDb != null)
                {
                    novaCompra.ListaFotografiasCompradas.Add(fotoDb);
                }
            }

            _context.Compras.Add(novaCompra);
            _context.SaveChanges();
        }

        HttpContext.Session.Remove("Carrinho");

        var comprasAtualizadas = _context.Compras
            .Include(c => c.ListaFotografiasCompradas)
            .Where(c => c.CompradorFK == utilizador.Id)
            .ToList();

        ViewBag.Compras = comprasAtualizadas;

        return View("Checkout", new CarrinhoViewModel
        {
            Nome = model.Nome,
            NIF = model.NIF,
            Fotografias = new List<Fotografias>()
        });
    }

    /// <summary>
    /// Adiciona uma fotografia ao carrinho de compras.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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

        return RedirectToAction("Loja", "Galeria");
    }

    public IActionResult Confirmacao()
    {
        return View();
    }
}
