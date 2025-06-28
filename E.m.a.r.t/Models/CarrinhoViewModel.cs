namespace E.m.a.r.t.Models
{
    // ViewModel para representar o carrinho de compras na aplicação
    public class CarrinhoViewModel
    {
        public string Nome { get; set; }  // Nome do utilizador
        public string NIF { get; set; }   // Número de identificação fiscal do utilizador
        public List<Fotografias> Fotografias { get; set; } = new(); // Lista de fotografias no carrinho, inicializada vazia
    }
}
