using E.m.a.r.t.Models;

/// <summary>
/// Modelo para o carrinho de compras do utilizador.
/// </summary>
public class CarrinhoViewModel
{
    public string Nome { get; set; }
    public string NIF { get; set; }
    public List<FotografiaCarrinhoDTO> Fotografias { get; set; } = new();
}