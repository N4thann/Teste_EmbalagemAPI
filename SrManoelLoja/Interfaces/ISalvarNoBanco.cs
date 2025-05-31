using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;

namespace SrManoelLoja.Interfaces
{
    public interface ISalvarNoBanco
    {
        Task<SalvarPedidosResponse> CriarRegistrosNoBanco(PedidosRequest request);
    }
}
