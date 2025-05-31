using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrManoelLoja.Entities
{
    [Table("ItensPedido")]
    public class ItemPedido
    {
        [Required]
        public Guid PedidoId { get; set; }
        [Required]
        public Guid ProdutoId { get; set; }

        public Pedido Pedido { get; set; }
        public Produto Produto { get; set; }

        protected ItemPedido() { }

        public ItemPedido(Guid pedidoId, Guid produtoId)
        {
            PedidoId = pedidoId;
            ProdutoId = produtoId;
        }
    }
}