using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Data
{
    /// <summary>
    /// Contexto da base de dados da aplicação, estende IdentityDbContext para suporte a autenticação e gestão de utilizadores.
    /// </summary>
    public class ApplicationDbContext : IdentityDbContext
    {
        /// <summary>
        /// Construtor que recebe opções para configurar o contexto.
        /// </summary>
        /// <param name="options">Opções do contexto.</param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Conjunto de entidades Fotografias na base de dados.
        /// </summary>
        public DbSet<Fotografias> Fotografias { get; set; }

        /// <summary>
        /// Conjunto de entidades Coleções na base de dados.
        /// </summary>
        public DbSet<Colecao> Colecoes { get; set; }

        /// <summary>
        /// Conjunto de entidades Utilizadores na base de dados.
        /// </summary>
        public DbSet<Utilizadores> Utilizadores { get; set; }

        /// <summary>
        /// Conjunto de entidades Compras na base de dados.
        /// </summary>
        public DbSet<Compras> Compras { get; set; }

        /// <summary>
        /// Método para configurar o modelo e as relações entre entidades.
        /// </summary>
        /// <param name="modelBuilder">Construtor de modelo do Entity Framework.</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais do modelo podem ser adicionadas aqui
        }
    }
}
