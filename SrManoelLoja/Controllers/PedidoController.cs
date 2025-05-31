using Microsoft.AspNetCore.Mvc;
using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;
using SrManoelLoja.Interfaces;

namespace SrManoelLoja.Controllers
{
    [ApiController]
    [Route("LojaManoel/api/[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly IEmbalagemService _servicoEmbalagem;
        private readonly ISalvarNoBanco _servicoSalvarBanco;

        public PedidoController(IEmbalagemService servicoEmbalagem, ISalvarNoBanco servicoSalvarBanco)
        {
            _servicoEmbalagem = servicoEmbalagem;
            _servicoSalvarBanco = servicoSalvarBanco;
        }

        [HttpPost("empacotar")]
        [ProducesResponseType(typeof(EmpacotarPedidosResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<IActionResult> EmpacotarPedidos([FromBody] PedidosRequest request)
        {
            if (request == null || !request.Pedidos.Any())
            {
                return BadRequest("A requisição deve conter uma lista de pedidos válida");
            }

            var embalagemResponse = await _servicoEmbalagem.ProcessarPedidos(request);

            if(embalagemResponse == null)
                return BadRequest("Erro no serviço de empacotamento");

            var salvarBancoResponse = await _servicoSalvarBanco.CriarRegistrosNoBanco(request);

            if (salvarBancoResponse == null)
                return BadRequest("Erro no serviço de salvar no banco");

            var response = new EmpacotarPedidosResponse(embalagemResponse, salvarBancoResponse);

            return Ok(response);            
        }
    }
}
