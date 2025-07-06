using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    public class Fotografias
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; } = string.Empty;

        [Display(Name = "Coleção")]
        public int? ColecaoFK { get; set; }

        [ForeignKey(nameof(ColecaoFK))]
        public Colecao? Colecao { get; set; }

        [Display(Name = "Descrição")]
        public string? Descricao { get; set; }

        [Required]
        [Display(Name = "Ficheiro da Imagem")]
        public string Ficheiro { get; set; } = string.Empty;

        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Preco { get; set; }
    }
}
