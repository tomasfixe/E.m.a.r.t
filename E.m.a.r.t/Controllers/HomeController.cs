using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Controllers;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ApplicationDbContext context, ILogger<HomeController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Mostrar formulário + tabela
    [HttpGet]
    public IActionResult Index()
    {
        var lista = _context.Fotografias.ToList();
        ViewBag.NovaFoto = new Fotografias(); // usado no formulário
        return View(lista);
    }

    // POST: Adicionar nova fotografia
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Index(Fotografias novaFoto)
    {
        if (ModelState.IsValid)
        {
            _context.Fotografias.Add(novaFoto);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        var lista = _context.Fotografias.ToList();
        ViewBag.NovaFoto = novaFoto;
        return View(lista);
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
