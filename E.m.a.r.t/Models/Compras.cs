using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace E.m.a.r.t.Models
{
    // Compras efetuadas por um utilizador
    public class Compras
    {
        [Key]
        public int Id { get; set; }  // Chave primária da compra

        public DateTime Data { get; set; }  // Data da compra

        public Estados Estado { get; set; }  // Estado atual da compra (enum)

        // FK para o comprador (ID do utilizador)
        [ForeignKey(nameof(Comprador))]
        public int CompradorFK { get; set; }

        [ValidateNever]
        public Utilizadores Comprador { get; set; } = null!; // Navegação para o comprador (utilizador)

        // Coleção de fotografias compradas nesta compra
        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; } = new List<Fotografias>();
    }

    // Estados possíveis associados a uma compra
    public enum Estados
    {
        Pendente,  // Compra ainda pendente
        Pago,      // Compra paga
        Enviada,   // Compra enviada
        Entregue,  // Compra entregue
        Terminada  // Compra finalizada/terminada
    }
}
