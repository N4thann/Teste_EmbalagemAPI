using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;
using SrManoelLoja.Entities;

namespace SrManoelLoja.Interfaces
{
    public interface IEmbalagemService
    {
        Task<PedidosResponse> ProcessarPedidos(PedidosRequest pedidosRequest);
    }
}
