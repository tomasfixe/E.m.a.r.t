using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{
    public class Fotografias
    {
        public int Id { get; set; }  // Identificador único da fotografia

        [Required(ErrorMessage = "O título é obrigatório.")]
        public string Titulo { get; set; }  // Título da fotografia, obrigatório

        // Chave estrangeira para a coleção a que esta foto pertence (pode ser nula)
        public int? ColecaoFK { get; set; }

        [ForeignKey(nameof(ColecaoFK))]
        public Colecao? Colecao { get; set; }  // Navegação para a coleção associada (pode ser nula)

        public string? Descricao { get; set; }  // Descrição opcional da fotografia

        [Display(Name = "Ficheiro da Imagem")]
        public string Ficheiro { get; set; } = string.Empty;  // Nome do ficheiro da imagem, inicializado como vazio

        [Display(Name = "Data da Foto")]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }  // Data associada à fotografia

        [Column(TypeName = "decimal(10,2)")]
        [Required(ErrorMessage = "O preço é obrigatório.")]
        public decimal Preco { get; set; }  // Preço da fotografia com precisão decimal, obrigatório
    }
}
