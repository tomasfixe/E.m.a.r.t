using System.Collections.Generic;

namespace E.m.a.r.t.Models
{
    /// <summary>
    /// ViewModel que representa os dados para a loja,
    /// incluindo coleções de fotografias e fotografias sem coleção.
    /// </summary>
    public class LojaViewModel
    {
        /// <summary>
        /// Lista de coleções de fotografias para a loja.
        /// </summary>
        public List<Colecao> Colecoes { get; set; }

        /// <summary>
        /// Lista de fotografias que não pertencem a nenhuma coleção.
        /// </summary>
        public List<Fotografias> FotografiasSemColecao { get; set; }
    }
}
