using Microsoft.EntityFrameworkCore;
using SrManoelLoja.Entities;

namespace SrManoelLoja.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Pedido> Pedidos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<ItemPedido> ItensPedido { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Produto>()
                .OwnsOne(p => p.Dimensoes, d =>
                {
                    d.Property(dim => dim.Altura).HasColumnName("Altura").IsRequired();
                    d.Property(dim => dim.Largura).HasColumnName("Largura").IsRequired();
                    d.Property(dim => dim.Comprimento).HasColumnName("Comprimento").IsRequired();
                });

            modelBuilder.Entity<ItemPedido>()
                .HasKey(ip => new { ip.PedidoId, ip.ProdutoId });

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Pedido)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(ip => ip.PedidoId);

            modelBuilder.Entity<ItemPedido>()
                .HasOne(ip => ip.Produto)
                .WithMany(prod => prod.ItensPedido)
                .HasForeignKey(ip => ip.ProdutoId);

            modelBuilder.Entity<Produto>()
                .HasIndex(p => p.Name) 
                .IsUnique();
        }
    }
}