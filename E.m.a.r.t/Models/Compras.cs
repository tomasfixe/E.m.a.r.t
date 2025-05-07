﻿using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace E.m.a.r.t.Models
{

    /// <summary>
    /// compras efetuadas por um utilizador
    /// </summary>
    public class Compras
    {

        /// <summary>
        /// Identificador da compra
        /// </summary>
        [Key]  // PK, int, autonumber
        public int Id { get; set; }

        /// <summary>
        /// Data da compra
        /// </summary>
        public DateTime Data { get; set; }

        /// <summary>
        /// Estado da compra.
        /// Representa um conjunto de valores pre-determinados
        /// que representam a evolução da 'compra'
        /// </summary>
        public Estados Estado { get; set; }



        /* *

  Definção dos relacionamentos
  ** */


        // Relacionamentos 1-N

        /// <summary>
        /// FK para referenciar o comprador da fotografia
        /// </summary>
        [ForeignKey(nameof(Comprador))]
        public int CompradorFK { get; set; }
        /// <summary>
        /// FK para referenciar o Comprador da fotografia
        /// </summary>
        [ValidateNever]
        public Utilizadores Comprador { get; set; } = null!;

        /// <summary>
        /// Lista das fotografias compradas pelo utilizador
        /// </summary>
        public ICollection<Fotografias> ListaFotografiasCompradas { get; set; } = [];
    }


    // https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum
    /// <summary>
    /// Estados associados a uma 'compra'
    /// </summary>
    public enum Estados
    {
        Pendente,
        Pago,
        Enviada,
        Entregue,
        Terminada
    }




}