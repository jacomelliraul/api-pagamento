namespace PagamentoAPI.Model.Entities
{
    public class Parcela
    {
        public int NumeroParcela { get; set; }
        public decimal Valor { get; set; }

        public Parcela(int _numeroParcela, decimal _valor)
        {
           NumeroParcela = _numeroParcela;
            Valor = _valor;
        }
    }
}
