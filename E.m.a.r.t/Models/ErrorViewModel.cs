namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Modelo usado para representar informa��es de erro na aplica��o.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Identificador da requisi��o que gerou o erro (pode ser nulo).
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indica se o RequestId deve ser exibido.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
