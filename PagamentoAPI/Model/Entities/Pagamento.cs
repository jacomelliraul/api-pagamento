namespace PagamentoAPI.Model.Entities
{
    public class Pagamento
    {
        public int transacaoId { get; set; }
        public int valor { get; set; }
        public string cartao{ get; set; }
        public int CVV { get; set; }
        public int parcelas { get; set; }
        public  int situacao { get; set; }
    }
}
