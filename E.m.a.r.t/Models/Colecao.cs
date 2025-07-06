using System.ComponentModel.DataAnnotations;

namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Representa uma coleção de fotografias.
    /// </summary>
    public class Colecao
    {
        /// <summary>
        /// Identificador único da coleção.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome da coleção (obrigatório).
        /// </summary>
        [Required(ErrorMessage = "O nome da coleção é obrigatório.")]
        public string Nome { get; set; } = "";

        /// <summary>
        /// Descrição opcional da coleção.
        /// </summary>
        public string? Descricao { get; set; }

        /// <summary>
        /// Lista das fotografias pertencentes a esta coleção.
        /// Inicializada como lista vazia.
        /// </summary>
        public ICollection<Fotografias> ListaFotografias { get; set; } = new List<Fotografias>();
    }
}
