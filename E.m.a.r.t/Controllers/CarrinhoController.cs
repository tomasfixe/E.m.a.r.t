using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;
using E.m.a.r.t.Helpers;
using System.Linq;

public class CarrinhoController : Controller
{
    private readonly ApplicationDbContext _context;

    public CarrinhoController(ApplicationDbContext context)
    {
        _context = context;
    }

    [Authorize]
    public IActionResult Checkout()
    {
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);
        var fotosCarrinho = HttpContext.Session.GetObjectFromJson<List<FotografiaCarrinhoDTO>>("Carrinho") ?? new List<FotografiaCarrinhoDTO>();

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
    /// Finaliza a compra das fotografias no carrinho e regista a compra no sistema.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult FinalizarCompra(CarrinhoViewModel model)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<FotografiaCarrinhoDTO>>("Carrinho") ?? new List<FotografiaCarrinhoDTO>();
        var utilizador = _context.Utilizadores.FirstOrDefault(u => u.UserName == User.Identity.Name);

        if (utilizador == null)
        {
            ModelState.AddModelError("", "Utilizador não autenticado.");
            // Recarrega a mesma view
            return View("Checkout", model);
        }

        if (!carrinho.Any())
        {
            ModelState.AddModelError("", "O carrinho está vazio.");
            return View("Checkout", model);
        }

        var novaCompra = new Compras
        {
            CompradorFK = utilizador.Id,
            Data = DateTime.Now,
            Estado = Estados.Pago,
            ListaFotografiasCompradas = new List<Fotografias>()
        };

        foreach (var fotoDto in carrinho)
        {
            var fotoDb = _context.Fotografias.Find(fotoDto.Id);
            if (fotoDb != null)
            {
                novaCompra.ListaFotografiasCompradas.Add(fotoDb);
            }
        }

        _context.Compras.Add(novaCompra);
        _context.SaveChanges();

        HttpContext.Session.Remove("Carrinho");

        // Passa as compras atualizadas para a view
        ViewBag.Compras = _context.Compras
            .Include(c => c.ListaFotografiasCompradas)
            .Where(c => c.CompradorFK == utilizador.Id)
            .ToList();

        // Podes querer limpar o model do carrinho aqui se estiveres a usar

        return View("Checkout", model);
    }

    /// <summary>
    /// Remove uma fotografia do carrinho de compras do utilizador.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize]
    public IActionResult RemoverDoCarrinho(int id)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<FotografiaCarrinhoDTO>>("Carrinho") ?? new List<FotografiaCarrinhoDTO>();

        var fotoARemover = carrinho.FirstOrDefault(f => f.Id == id);
        if (fotoARemover != null)
        {
            carrinho.Remove(fotoARemover);

            if (carrinho.Any())
                HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            else
                HttpContext.Session.Remove("Carrinho");
        }

        return RedirectToAction("Checkout");
    }

    [HttpPost]
    public IActionResult Adicionar(int id)
    {
        var carrinho = HttpContext.Session.GetObjectFromJson<List<FotografiaCarrinhoDTO>>("Carrinho") ?? new List<FotografiaCarrinhoDTO>();

        if (!carrinho.Any(f => f.Id == id))
        {
            var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
            if (foto != null)
            {
                var fotoDto = new FotografiaCarrinhoDTO
                {
                    Id = foto.Id,
                    Titulo = foto.Titulo,
                    Ficheiro = foto.Ficheiro,
                    Preco = foto.Preco
                };
                carrinho.Add(fotoDto);
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
