using System.ComponentModel.DataAnnotations;

namespace E.m.a.r.t.Models
{
    public class Colecao
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome da coleção é obrigatório.")]
        public string Nome { get; set; } = "";

        public string? Descricao { get; set; }

        // Lista de fotografias nesta coleção 1-N
        public ICollection<Fotografias> ListaFotografias { get; set; } = new List<Fotografias>();
    }
}
