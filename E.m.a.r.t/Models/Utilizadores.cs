using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace E.m.a.r.t.Models
{

    public class Utilizadores
    {
        
        // Identificador do utilizador
        [Key]
        public int Id { get; set; }

     
        // Nome do utilizador
  
        [Display(Name = "Nome")]
        [StringLength(50)]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string Nome { get; set; } = string.Empty;


        // Morada do utilizador
    
        [Display(Name = "Morada")]
        [StringLength(50)]
        public string? Morada { get; set; }

     
        // Código Postal da morada do utilizador
    
        [Display(Name = "Código Postal")]
        [StringLength(50)]
        [RegularExpression("[1-9][0-9]{3}-[0-9]{3} [A-Za-z ]+",
            ErrorMessage = "No {0} só são aceites algarismos e letras inglesas.")]
        public string? CodPostal { get; set; }

      
        // País da morada do utilizador
    
        [Display(Name = "País")]
        [StringLength(50)]
        public string? Pais { get; set; }

     
        // Número de identificação fiscal do Utilizador
       
        [Display(Name = "NIF")]
        [StringLength(9)]
        [RegularExpression("[1-9][0-9]{8}", ErrorMessage = "Deve escrever apenas 9 digitos no {0}")]
        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório")]
        public string NIF { get; set; } = string.Empty;

  
        // Número de telemóvel do utilizador
       
        [Display(Name = "Telemóvel")]
        [StringLength(18)]
        [RegularExpression("(([+]|00)[0-9]{1,5})?[1-9][0-9]{5,10}", ErrorMessage = "Escreva um nº de telefone. Pode adicionar indicativo do país.")]
        public string? Telemovel { get; set; }

       
        // Ponte para a tabela de autenticação (Identity)
       
        [StringLength(50)]
        public string UserName { get; set; } = string.Empty;

        // Relacionamentos

      
        // Fotografias do utilizador
       
        public ICollection<Fotografias> ListaFotos { get; set; } = [];

      
        // Fotografias compradas pelo utilizador
      
        public ICollection<Compras> ListaCompras { get; set; } = [];
    }
}
