using System.Collections.Generic;

namespace E.m.a.r.t.Models
{
    public class LojaViewModel
    {
        public List<Colecao> Colecoes { get; set; }  // Lista de coleções de fotografias para a loja
        public List<Fotografias> FotografiasSemColecao { get; set; }  // Fotografias que não pertencem a nenhuma coleção
    }
}
