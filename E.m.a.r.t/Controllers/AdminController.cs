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

    /// <summary>
    /// Página de upload de fotografias e gestão de coleções.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Método para upload de uma nova fotografia.
    /// </summary>
    /// <param name="novaFoto"></param>
    /// <param name="imagem"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Upload(Fotografias novaFoto, IFormFile imagem)
    {
        if (imagem == null || imagem.Length == 0)
        {
            ModelState.AddModelError("imagem", "O ficheiro da imagem é obrigatório.");
        }

        if (ModelState.IsValid)
        {
            var nomeFicheiro = Guid.NewGuid().ToString() + Path.GetExtension(imagem.FileName);
            var caminho = Path.Combine("wwwroot/imagens", nomeFicheiro);

            using (var stream = new FileStream(caminho, FileMode.Create))
            {
                imagem.CopyTo(stream);
            }

            novaFoto.Ficheiro = nomeFicheiro;

            _context.Fotografias.Add(novaFoto);
            _context.SaveChanges();

            return RedirectToAction("Upload");
        }

        // Se chegar aqui, há erros — recarregar view com dados
        ViewBag.ListaColecoes = _context.Colecoes.ToList();
        ViewBag.ListaFotos = _context.Fotografias.ToList();
        ViewBag.NovaFoto = novaFoto;

        var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
        var novaColecao = new Colecao();

        return View(Tuple.Create(lista.AsEnumerable(), novaColecao));
    }

    /// <summary>
    /// Método para editar uma fotografia existente.
    /// </summary>
    /// <param name="fotografiaAtualizada"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditarFotografia(Fotografias fotografiaAtualizada)
    {
        if (ModelState.IsValid)
        {
            var fotografiaExistente = _context.Fotografias.FirstOrDefault(f => f.Id == fotografiaAtualizada.Id);
            if (fotografiaExistente == null)
            {
                return NotFound();
            }

            fotografiaExistente.Titulo = fotografiaAtualizada.Titulo;
            fotografiaExistente.Descricao = fotografiaAtualizada.Descricao;
            fotografiaExistente.Preco = fotografiaAtualizada.Preco;
            fotografiaExistente.ColecaoFK = fotografiaAtualizada.ColecaoFK;

            _context.SaveChanges();
            return RedirectToAction("Upload");
        }

        ViewBag.ListaColecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
        ViewBag.ListaFotos = _context.Fotografias.Include(f => f.Colecao).ToList();
        ViewBag.NovaFoto = new Fotografias();

        var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
        var novaColecao = new Colecao();

        return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
    }

    /// <summary>
    /// Método para criar uma nova coleção de fotografias.
    /// </summary>
    /// <param name="novaColecao"></param>
    /// <param name="ListaFotografiasIds"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CriarColecao([Bind(Prefix = "Item2")] Colecao novaColecao, int[] ListaFotografiasIds)
    {
        if (ListaFotografiasIds == null || ListaFotografiasIds.Length == 0)
        {
            ModelState.AddModelError("", "É obrigatório associar pelo menos uma fotografia à coleção.");
        }

        if (ModelState.IsValid)
        {
            _context.Colecoes.Add(novaColecao);
            _context.SaveChanges();

            var fotosParaAtualizar = _context.Fotografias.Where(f => ListaFotografiasIds.Contains(f.Id)).ToList();

            foreach (var foto in fotosParaAtualizar)
            {
                foto.ColecaoFK = novaColecao.Id;
            }

            _context.SaveChanges();

            return RedirectToAction("Upload");
        }

        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine("Erro: " + error.ErrorMessage);
        }

        ViewBag.ListaColecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
        ViewBag.ListaFotos = _context.Fotografias.ToList();
        ViewBag.NovaFoto = new Fotografias();

        var lista = _context.Fotografias.Include(f => f.Colecao).ToList();
        return View("Upload", Tuple.Create(lista.AsEnumerable(), novaColecao));
    }

    /// <summary>
    /// Método para eliminar uma coleção de fotografias.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarColecao(int id)
    {
        var colecao = _context.Colecoes
            .Include(c => c.ListaFotografias)
            .FirstOrDefault(c => c.Id == id);

        if (colecao == null)
        {
            return NotFound();
        }

        foreach (var foto in colecao.ListaFotografias)
        {
            foto.ColecaoFK = null;
        }

        _context.Colecoes.Remove(colecao);
        _context.SaveChanges();

        return RedirectToAction("Upload");
    }

    /// <summary>
    /// Método para apagar uma fotografia específica.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ApagarFotografia(int id)
    {
        var foto = _context.Fotografias.FirstOrDefault(f => f.Id == id);
        if (foto == null)
        {
            return NotFound();
        }

        // Apagar o ficheiro físico (opcional)
        var caminho = Path.Combine("wwwroot/imagens", foto.Ficheiro);
        if (System.IO.File.Exists(caminho))
        {
            System.IO.File.Delete(caminho);
        }

        _context.Fotografias.Remove(foto);
        _context.SaveChanges();

        return RedirectToAction("Upload");
    }

}
