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
}
