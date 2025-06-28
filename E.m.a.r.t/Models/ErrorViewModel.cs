namespace E.m.a.r.t.Models
{
    // Modelo usado para representar informa��es de erro na aplica��o
    public class ErrorViewModel
    {
        // Id da requisi��o que gerou o erro (pode ser nulo)
        public string? RequestId { get; set; }

        // Propriedade que indica se o RequestId deve ser exibido
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
