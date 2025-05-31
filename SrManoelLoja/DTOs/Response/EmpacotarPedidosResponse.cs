namespace SrManoelLoja.DTOs.Response
{
    public record EmpacotarPedidosResponse(
        PedidosResponse EmbalagemResultado,
        SalvarPedidosResponse PersistenciaResultado)
    {
        public bool SucessoGlobal =>
            (EmbalagemResultado != null && EmbalagemResultado.Pedidos != null && EmbalagemResultado.Pedidos.Any()) &&
            (PersistenciaResultado != null && (PersistenciaResultado.PedidosAdicionados > 0 || PersistenciaResultado.ProdutosAdicionados > 0));
    }
}
