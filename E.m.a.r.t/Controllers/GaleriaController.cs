﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Data;
using E.m.a.r.t.Models;

public class GaleriaController : Controller
{
   
    private readonly ApplicationDbContext _context;

    public GaleriaController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Página principal da galeria, exibindo todas as coleções e fotografias.
    /// </summary>
    /// <returns></returns>
    public IActionResult Index()
    {
        var colecoes = _context.Colecoes.Include(c => c.ListaFotografias).ToList();
        return View(colecoes);
    }

    /// <summary>
    /// Página principal da loja, exibindo coleções e fotografias sem coleção.
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// Detalhes de uma fotografia específica.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
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
}
