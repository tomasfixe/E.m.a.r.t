using Microsoft.AspNetCore.Mvc;
using E.m.a.r.t.Data;
using Microsoft.EntityFrameworkCore;

public class HomeController : Controller
{
    private readonly ApplicationDbContext _context;

    public HomeController(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var colecoes = await _context.Colecoes
            .Include(c => c.ListaFotografias)
            .ToListAsync();

        return View(colecoes);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
