using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    public class Fotografias
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }

        // FK para coleção
        public int? ColecaoFK { get; set; }

        [ForeignKey(nameof(ColecaoFK))]
        public Colecao? Colecao { get; set; }

        public string? Descricao { get; set; }

        [Display(Name = "Ficheiro da Imagem")]
        
        public string Ficheiro { get; set; } = string.Empty;

        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        public decimal Preco { get; set; }

    }
}
