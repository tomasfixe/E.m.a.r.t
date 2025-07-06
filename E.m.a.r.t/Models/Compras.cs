using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;

namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Representa uma compra efetuada por um utilizador.
    /// </summary>
    public class Compras
    {
        /// <summary>
        /// Chave primária da compra.
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Data em que a compra foi efetuada.
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Estado atual da compra.
        /// </summary>
        public Estados Estado { get; set; }

        /// <summary>
        /// Chave estrangeira para o comprador (utilizador).
        /// </summary>
        [ForeignKey(nameof(Comprador))]
        public int CompradorFK { get; set; }

        /// <summary>
        /// Navegação para o utilizador comprador.
        /// </summary>
        [ValidateNever]
        public Utilizadores Comprador { get; set; } = null!;

        /// <summary>
        /// Lista das fotografias compradas nesta compra.
        /// </summary>
        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; } = new List<Fotografias>();
    }

    /// <summary>
    /// Estados possíveis de uma compra.
    /// </summary>
    public enum Estados
    {
        /// <summary>Compra ainda pendente.</summary>
        Pendente,

        /// <summary>Compra paga.</summary>
        Pago,

        /// <summary>Compra enviada.</summary>
        Enviada,

        /// <summary>Compra entregue.</summary>
        Entregue,

        /// <summary>Compra finalizada/terminada.</summary>
        Terminada
    }
}
