using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using E.m.a.r.t.Models;

namespace E.m.a.r.t.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Fotografias> Fotografias { get; set; }
        public DbSet<Colecao> Colecoes { get; set; }
        public DbSet<Utilizadores> Utilizadores { get; set; }
        public DbSet<Compras> Compras { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
