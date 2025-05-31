using SrManoelLoja.SeedWork.Validate;
using SrManoelLoja.ValueObjects;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace SrManoelLoja.Entities
{
    [Table("Produtos")]
    public class Produto
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; private set; }

        [Required]
        [StringLength(50)] // Name agora é o identificador único de catálogo e descrição
        public string Name { get; private set; }

        public Dimensao Dimensoes { get; private set; }

        // Relacionamento com a entidade de junção ItemPedido
        public virtual ICollection<ItemPedido> ItensPedido { get; private set; }

        // Construtor padrão para Entity Framework
        protected Produto() { }

        // Construtor atualizado: remove codProd
        public Produto(string name, Dimensao dimensao)
        {
            Id = Guid.NewGuid();
            Validate.NotNullOrEmpty(name, nameof(name));
            Validate.MaxLength(name, 50, nameof(name)); // Validação de domínio para Name
            Validate.NotNull(dimensao, nameof(dimensao));

            Name = name;
            Dimensoes = dimensao;
            ItensPedido = new List<ItemPedido>();
        }
    }
}