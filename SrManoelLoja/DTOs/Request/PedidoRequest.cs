namespace SrManoelLoja.DTOs.Request
{
    public class PedidoRequest
    {
        public int Pedido_id { get; set; }
        public List<ProdutoRequest> Produtos { get; set; }
    }
}
