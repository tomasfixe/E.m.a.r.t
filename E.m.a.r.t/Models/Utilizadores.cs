using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Representa um utilizador da aplicação com os seus dados pessoais, contactos e relações.
    /// </summary>
    public class Utilizadores
    {
        /// <summary>
        /// Identificador único do utilizador.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Nome do utilizador (máximo 50 caracteres, obrigatório).
        /// </summary>
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Morada do utilizador (máximo 50 caracteres, opcional).
        /// </summary>
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string? Morada { get; set; }

        /// <summary>
        /// Código Postal do utilizador (formato específico, opcional).
        /// </summary>
        [Display(Name = "Código Postal")]
        [StringLength(50)]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+",
            ErrorMessage = "No {0} só são aceites algarismos e letras inglesas.")]
        public string? CodPostal { get; set; }

        /// <summary>
        /// País da morada do utilizador (máximo 50 caracteres, opcional).
        /// </summary>
        [Display(Name = "País")]
        [StringLength(50)]
        public string? Pais { get; set; }

        /// <summary>
        /// Número de identificação fiscal (NIF) do utilizador (9 dígitos, obrigatório).
        /// </summary>
        [Display(Name = "NIF")]
        [StringLength(9)]
        [RegularExpression("[1-9][0-9]{8}", ErrorMessage = "Deve escrever apenas 9 dígitos no {0}")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string NIF { get; set; } = string.Empty;

        /// <summary>
        /// Número de telemóvel do utilizador (opcional, pode incluir indicativo internacional).
        /// </summary>
        [Display(Name = "Telemóvel")]
        [StringLength(18)]
        [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}", ErrorMessage = "Escreva um nº de telefone. Pode adicionar indicativo do país.")]
        public string? Telemovel { get; set; }

        /// <summary>
        /// Nome de utilizador para autenticação (máximo 50 caracteres).
        /// </summary>
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Coleção de fotografias associadas ao utilizador.
        /// </summary>
        public ICollection<Fotografias> ListaFotos { get; set; } = new List<Fotografias>();

        /// <summary>
        /// Coleção de compras realizadas pelo utilizador.
        /// </summary>
        public ICollection<Compras> ListaCompras { get; set; } = new List<Compras>();
    }
}
