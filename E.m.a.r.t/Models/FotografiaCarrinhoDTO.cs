namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Versão leve da fotografia para usar na sessão do carrinho.
    /// </summary>
    public class FotografiaCarrinhoDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Ficheiro { get; set; } = string.Empty;
        public decimal Preco { get; set; }
    }
}