using SrManoelLoja.SeedWork;
using SrManoelLoja.SeedWork.Validate;
using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SrManoelLoja.Entities
{
    [Table("Pedidos")]
    public class Pedido
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        [Required]
        public int PedidoExterno_id { get; private set; }

        private readonly List<ItemPedido> _itensPedido;

        protected Pedido()
        {
            Id = Guid.NewGuid();
            _itensPedido = new List<ItemPedido>();
        }

        public Pedido(int pedidoExternoId) : this()
        {
            Validate.GreaterThan(pedidoExternoId, 0, nameof(pedidoExternoId));
            PedidoExterno_id = pedidoExternoId;           
        }

        public virtual ICollection<ItemPedido> ItensPedido => _itensPedido;

        public void AddItemPedido(ItemPedido itemPedido)
        {
            Validate.NotNull(itemPedido, nameof(itemPedido));
            _itensPedido.Add(itemPedido);
        }
    }
}