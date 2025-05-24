using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    public class Fotografias
    {
        public int Id { get; set; }

        [Required]
        public string Titulo { get; set; }

        public string? Descricao { get; set; }

        [Display(Name = "Ficheiro da Imagem")]
        public string Ficheiro { get; set; }

        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        [Range(0.01, 9999999.99, ErrorMessage = "Preço inválido.")]
        public decimal Preco { get; set; }

    }
}
