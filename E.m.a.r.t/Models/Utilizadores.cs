using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace E.m.a.r.t.Models
{
    public class Utilizadores
    {
        // Identificador único do utilizador
        [Key]
        public int Id { get; set; }

        // Nome do utilizador (máx 50 caracteres, obrigatório)
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Nome { get; set; } = string.Empty;

        // Morada do utilizador (máx 50 caracteres, opcional)
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string? Morada { get; set; }

        // Código Postal do utilizador (formato específico, opcional)
        [Display(Name = "Código Postal")]
        [StringLength(50)]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+",
            ErrorMessage = "No {0} só são aceites algarismos e letras inglesas.")]
        public string? CodPostal { get; set; }

        // País da morada do utilizador (máx 50 caracteres, opcional)
        [Display(Name = "País")]
        [StringLength(50)]
        public string? Pais { get; set; }

        // Número de identificação fiscal (NIF) do utilizador (9 dígitos, obrigatório)
        [Display(Name = "NIF")]
        [StringLength(9)]
        [RegularExpression("[1-9][0-9]{8}", ErrorMessage = "Deve escrever apenas 9 digitos no {0}")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string NIF { get; set; } = string.Empty;

        // Número de telemóvel do utilizador (opcional, pode incluir indicativo internacional)
        [Display(Name = "Telemóvel")]
        [StringLength(18)]
        [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}", ErrorMessage = "Escreva um nº de telefone. Pode adicionar indicativo do país.")]
        public string? Telemovel { get; set; }

        // Username utilizado na autenticação (máx 50 caracteres)
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        // Coleção de fotografias associadas ao utilizador
        public ICollection<Fotografias> ListaFotos { get; set; } = [];

        // Coleção de compras efetuadas pelo utilizador
        public ICollection<Compras> ListaCompras { get; set; } = [];
    }
}
