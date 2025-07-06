namespace E.m.a.r.t.Models
{
    /// <summary>
    /// Modelo usado para representar informações de erro na aplicação.
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Identificador da requisição que gerou o erro (pode ser nulo).
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Indica se o RequestId deve ser exibido.
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
