using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    public class Fotografias
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }

        public string? Descricao { get; set; }

        [Display(Name = "Ficheiro da Imagem")]
        [Required(ErrorMessage = "O ficheiro da imagem é obrigatório.")]
        public string Ficheiro { get; set; }

        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        public decimal Preco { get; set; }

    }
}
