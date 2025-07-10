namespace E.m.a.r.t.Models
{
    /// <summary>
    /// ViewModel que representa o carrinho de compras na aplicação.
    /// Contém os dados do utilizador e as fotografias selecionadas.
    /// </summary>
    public class CarrinhoViewModel
    {
        /// <summary>
        /// Nome do utilizador associado ao carrinho.
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Número de Identificação Fiscal (NIF) do utilizador.
        /// </summary>
        public string NIF { get; set; }

        /// <summary>
        /// Lista das fotografias presentes no carrinho.
        /// Inicializada vazia para evitar null reference.
        /// </summary>
        public List<Fotografias> Fotografias { get; set; } = new();
    }
}