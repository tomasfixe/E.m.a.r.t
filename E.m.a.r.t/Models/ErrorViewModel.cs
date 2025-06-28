namespace E.m.a.r.t.Models
{
    // Modelo usado para representar informações de erro na aplicação
    public class ErrorViewModel
    {
        // Id da requisição que gerou o erro (pode ser nulo)
        public string? RequestId { get; set; }

        // Propriedade que indica se o RequestId deve ser exibido
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
