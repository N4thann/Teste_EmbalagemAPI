namespace SrManoelLoja.DTOs.Response
{
    public class PedidoResponse
    {
        public int Pedido_Id { get; set; }
        public List<CaixaResponse> Caixas { get; set; }
    }
}
