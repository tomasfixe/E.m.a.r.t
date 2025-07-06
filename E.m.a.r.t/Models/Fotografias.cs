using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Representa uma fotografia com os seus detalhes e ligação a uma coleção.
    /// </summary>
    public class Fotografias
    {
        /// <summary>
        /// Chave primária da fotografia.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Título da fotografia (obrigatório).
        /// </summary>
        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        /// <summary>
        /// Chave estrangeira para a coleção a que esta fotografia pertence (opcional).
        /// </summary>
        [Display(Name = "Coleção")]
        public int? ColecaoFK { get; set; }

        /// <summary>
        /// Referência à coleção associada.
        /// </summary>
        [ForeignKey(nameof(ColecaoFK))]
        public Colecao? Colecao { get; set; }

        /// <summary>
        /// Descrição opcional da fotografia.
        /// </summary>
        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        /// <summary>
        /// Nome do ficheiro da imagem (obrigatório).
        /// </summary>
        [Required]
        [Display(Name = "Ficheiro da Imagem")]
        public string Ficheiro { get; set; } = string.Empty;

        /// <summary>
        /// Data da fotografia.
        /// </summary>
        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; } = DateTime.Now;

        /// <summary>
        /// Preço da fotografia (obrigatório).
        /// </summary>
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
    }
}
