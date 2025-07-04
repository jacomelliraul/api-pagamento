using Microsoft.AspNetCore.Mvc;
using PagamentoAPI.Model.DTOs;
using PagamentoAPI.Model.Entities;
using PagamentoAPI.Services;

namespace PagamentoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PagamentosController : ControllerBase
    {
        private readonly PagamentoService _service;
        private readonly ILogger<PagamentosController> _logger;

        public PagamentosController(PagamentoService pagamentoService, ILogger<PagamentosController> logger)
        {
            _service = pagamentoService;
            _logger = logger;
        }

        /// <summary>
        /// Calcular Parcelas
        /// </summary>
        /// <param name="parcelasDTO"></param>
        /// <returns></returns>
        [HttpPost("calcular-parcelas")]
        public ActionResult CalcularParcelas(ParcelasDTO parcelasDTO)
        {
            List<Parcela> parcelas = null;
            try
            {   
                parcelas = _service.CalcularParcelas(parcelasDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError("Erro ao calcular parcelas");
            }

            return Ok(parcelas);
        }

        /// <summary>
        /// Efetuar um pagamento
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CriarPagamento([FromBody] PagamentoRequestDTO dto)
        {
            var id = _service.ProcessarPagamento(dto);

            if (id == 0)
                return BadRequest("Cartão inválido.");

            return CreatedAtAction(nameof(CriarPagamento), new { id = id }, new { id });
        }

        /// <summary>
        /// Consultar situação da transação
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/situacao")]
        public IActionResult ConsultarSituacao(int id)
        {
            var situacao = _service.ConsultarSituacao(id);

            if (situacao == null)
                return NotFound("Pagamento não encontrado.");

            return Ok(situacao);
        }

        /// <summary>
        /// Confirmar pagamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/confirmar")]
        public IActionResult ConfirmarPagamento(int id)
        {
            if (!_service.ConfirmarPagamento(id, out string erro))
                return BadRequest(erro);

            return Ok("Pagamento confirmado com sucesso.");
        }

        /// <summary>
        /// Cancelar pagamento
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("{id}/cancelar")]
        public IActionResult CancelarPagamento(int id)
        {
            if (!_service.CancelarPagamento(id, out string erro))
                return BadRequest(erro);

            return Ok("Pagamento cancelado com sucesso.");
        }




    }
}
