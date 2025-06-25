namespace E.m.a.r.t.Models
{
    public class CarrinhoViewModel
    {
        public string Nome { get; set; }
        public string NIF { get; set; }
        public string Morada { get; set; }
        public List<Fotografias> Fotografias { get; set; } = new();
    }
}
