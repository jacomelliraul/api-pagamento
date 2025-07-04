using PagamentoAPI.Repository;

namespace PagamentoAPI.Services
{
    public class CartaoService
    {
        private readonly CartaoRepository _repository;

        public CartaoService(CartaoRepository cartaoRepository)
        {
            _repository = cartaoRepository;
        }
        public string ObterBandeira(string numeroCartao)
        {
            if (string.IsNullOrWhiteSpace(numeroCartao) || numeroCartao.Length < 8)
                return null;

            string binPrefix = numeroCartao.Substring(0, 4);
            char nonoDigito = numeroCartao[10];

            if (binPrefix == "1111" && nonoDigito == '1')
                return "VISA";
            if (binPrefix == "2222" && nonoDigito == '2')
                return "MASTERCARD";
            if (binPrefix == "3333" && nonoDigito == '3')
                return "ELO";

            return null;
        }

        public bool CartaoValido(string numeroCartao)
        {
            return _repository.VerificarCartaoValido(numeroCartao);
        }
    }
}
