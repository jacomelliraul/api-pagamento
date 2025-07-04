using PagamentoAPI.Model.DTOs;
using PagamentoAPI.Model.Entities;
using PagamentoAPI.Model.Enum;
using PagamentoAPI.Repository;

namespace PagamentoAPI.Services
{
    public class PagamentoService
    {
        private readonly PagamentoRepository _pagamentoRepository;
        private readonly CartaoRepository _cartaoRepository;


        public PagamentoService(PagamentoRepository pagamentoRepository, CartaoRepository cartaoRepository)
        {
            _pagamentoRepository = pagamentoRepository;
            _cartaoRepository = cartaoRepository;
        }
        public List<Parcela> CalcularParcelas(ParcelasDTO parcelasDTO)
        {
            var total = parcelasDTO.ValorTotal * parcelasDTO.TaxaJuros;

            var totalParcela = total / parcelasDTO.QntdParcelas;

            List<Parcela> parcelas = new List<Parcela>();

            for(var i = 0; i< parcelasDTO.QntdParcelas; i++)
            {
                parcelas.Add(new Parcela(i+1, totalParcela));
            }

            return parcelas;
        }

        public int ProcessarPagamento(PagamentoRequestDTO dto)
        {
            bool cartaoValido = _cartaoRepository.VerificarCartaoValido(dto.NumeroCartao);

            if (!cartaoValido)
                return 0;

            var pagamento = new Pagamento
            {
                valor = (int)dto.Valor,
                cartao = dto.NumeroCartao,
                CVV = dto.CVV,
                parcelas = dto.Parcelas,
                situacao = (int)StatusPagamento.PENDENTE
            };

            return _pagamentoRepository.Adicionar(pagamento);
        }

        public string ConsultarSituacao(int id)
        {
            var status = _pagamentoRepository.ObterSituacaoPorId(id);

            return status?.ToString();
        }

        public bool ConfirmarPagamento(int id, out string erro)
        {
            erro = "";

            var situacaoAtual = _pagamentoRepository.ObterSituacaoPorId(id);

            if (situacaoAtual == null)
            {
                erro = "Pagamento não encontrado.";
                return false;
            }

            if (situacaoAtual == StatusPagamento.CANCELADO)
            {
                erro = "Pagamento cancelado, não pode ser confirmado.";
                return false;
            }

            if (situacaoAtual == StatusPagamento.CONFIRMADO)
            {
                erro = "Pagamento já está confirmado.";
                return false;
            }

            return _pagamentoRepository.AtualizarSituacao(id, StatusPagamento.CONFIRMADO);
        }

        public bool CancelarPagamento(int id, out string erro)
        {
            erro = "";

            var situacaoAtual = _pagamentoRepository.ObterSituacaoPorId(id);

            if (situacaoAtual == null)
            {
                erro = "Pagamento não encontrado.";
                return false;
            }

            if (situacaoAtual == StatusPagamento.CONFIRMADO)
            {
                erro = "Pagamento já confirmado não pode ser cancelado.";
                return false;
            }

            if (situacaoAtual == StatusPagamento.CANCELADO)
            {
                erro = "Pagamento já está cancelado.";
                return false;
            }

            return _pagamentoRepository.AtualizarSituacao(id, StatusPagamento.CANCELADO);
        }




    }
}
