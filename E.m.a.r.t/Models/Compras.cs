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
        public int Id { get; set; }

        public DateTime Data { get; set; }

        public Estados Estado { get; set; }

        // FK para o comprador
        [ForeignKey(nameof(Comprador))]
        public int CompradorFK { get; set; }

        [ValidateNever]
        public Utilizadores Comprador { get; set; } = null!;

        // Fotografias compradas nesta compra
        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; } = [];
    }

    
    // Estados associados a uma compra

    public enum Estados
    {
        Pendente,
        Pago,
        Enviada,
        Entregue,
        Terminada
    }
}