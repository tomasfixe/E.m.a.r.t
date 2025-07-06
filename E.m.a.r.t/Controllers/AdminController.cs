using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

[Authorize(Roles = "Admin")]
public class AdminController : Controller
{
    private readonly ApplicationDbContext _context;

    public AdminController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Upload()
    {
        ViewBag.ListaColecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
        ViewBag.ListaFotos = _context.Fotografias.ToList();
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
        ViewBag.ListaFotos = _context.Fotografias.ToList();
        ViewBag.NovaFoto = novaFoto;

        var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
        var novaColecao = new Colecao();

        return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
    }

    // Mantém os métodos Delete, Edit, EditarColecao, EliminarColecao...

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CriarColecao(Colecao novaColecao)
    {
        // Validação: garantir que tenha pelo menos 1 foto na lista
        if (novaColecao.ListaFotografias == null || !novaColecao.ListaFotografias.Any())
        {
            ModelState.AddModelError("", "É obrigatório associar pelo menos uma fotografia à coleção.");
        }

        if (ModelState.IsValid)
        {
            _context.Colecoes.Add(novaColecao);
            _context.SaveChanges();
            return RedirectToAction("Upload");
        }

        ViewBag.ListaColecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList(); // ajustei para ListaFotografias
        ViewBag.ListaFotos = _context.Fotografias.ToList();
        ViewBag.NovaFoto = new Fotografias();

        var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
        return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
    }
}
