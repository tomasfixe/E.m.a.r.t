using System.ComponentModel.DataAnnotations;

namespace E.m.a.r.t.Models
{
    public class Colecao
    {
        [Key]
        public int Id { get; set; }  // Chave primária da coleção

        [Required(ErrorMessage = "O nome da coleção é obrigatório.")]
        public string Nome { get; set; } = "";  // Nome da coleção, obrigatório

        public string? Descricao { get; set; }  // Descrição opcional da coleção

        // Relação 1-N: uma coleção pode conter várias fotografias
        public ICollection<Fotografias> ListaFotografias { get; set; } = new List<Fotografias>();
    }
}
