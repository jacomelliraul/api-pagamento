using Microsoft.AspNetCore.Mvc;
using PagamentoAPI.Model.DTOs;
using PagamentoAPI.Services;

namespace PagamentoAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CartoesController : ControllerBase
    {
        private readonly CartaoService _service;
        private readonly ILogger<CartoesController> _logger;

        public CartoesController(CartaoService cartaoService, ILogger<CartoesController> logger)
        {
            _service = cartaoService;
            _logger = logger;
        }

        /// <summary>
        /// Obtem a bandeira do Cartao
        /// </summary>
        /// <param name="numeroCartao"></param>
        /// <returns></returns>
        [HttpGet("{cartao}/obter-bandeira")]
        public ActionResult ObterBandeira(string cartao)
        {
            var bandeira = _service.ObterBandeira(cartao);

            try
            {
                if (bandeira == null)
                    return NotFound("Bandeira desconhecida");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao ObterBandeira");
            }

            return Ok(new BandeiraCartaoResponseDTO { Bandeira = bandeira });

        }

        /// <summary>
        /// Verificar cartão válido
        /// </summary>
        /// <param name="numeroCartao"></param>
        /// <returns></returns>
        [HttpGet("{cartao}/valido")]
        public ActionResult VerificarValidade(string cartao)
        {
            bool valido = false;
            try
            {
                valido = _service.CartaoValido(cartao);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao validar cartão!");
            }
            
            return Ok(valido); // Retorna true ou false direto
        }
    }
}
