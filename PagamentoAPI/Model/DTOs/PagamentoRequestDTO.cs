namespace PagamentoAPI.Model.DTOs
{
    public class PagamentoRequestDTO
    {
        public decimal Valor { get; set; }
        public string NumeroCartao { get; set; }
        public int CVV { get; set; }
        public int Parcelas { get; set; }
    }

}
