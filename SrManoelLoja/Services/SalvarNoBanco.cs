using Microsoft.EntityFrameworkCore;
using SrManoelLoja.Data;
using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;
using SrManoelLoja.Entities;
using SrManoelLoja.Interfaces;
using SrManoelLoja.ValueObjects;

namespace SrManoelLoja.Services
{
    public class SalvarNoBanco : ISalvarNoBanco
    {
        private readonly ApplicationDbContext _context;

        public SalvarNoBanco(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<SalvarPedidosResponse> CriarRegistrosNoBanco(PedidosRequest request)
        {
            var response = new SalvarPedidosResponse();

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                foreach (var pedidoRequest in request.Pedidos)
                {
                    // 1. Encontrar ou criar o Pedido
                    var pedidoDoBanco = await _context.Pedidos
                                                      .Include(p => p.ItensPedido)
                                                          .ThenInclude(ip => ip.Produto)
                                                      .FirstOrDefaultAsync(p => p.PedidoExterno_id == pedidoRequest.Pedido_id);

                    if (pedidoDoBanco == null)
                    {
                        pedidoDoBanco = new Pedido(pedidoRequest.Pedido_id);
                        await _context.Pedidos.AddAsync(pedidoDoBanco);
                        response.PedidosAdicionados++;
                    }
                    else
                    {
                        response.PedidosIgnorados++;
                    }

                    foreach (var produtoRequest in pedidoRequest.Produtos)
                    {                 
                        var produtoDoCatalogo = await _context.Produtos
                                                              .FirstOrDefaultAsync(p => p.Name == produtoRequest.Produto_id);

                        if (produtoDoCatalogo == null)
                        {
                            var dimensao = new Dimensao(produtoRequest.Dimensoes.Altura, produtoRequest.Dimensoes.Largura, produtoRequest.Dimensoes.Comprimento);
                            produtoDoCatalogo = new Produto(produtoRequest.Produto_id, dimensao);
                            await _context.Produtos.AddAsync(produtoDoCatalogo);
                        }

                        var itemPedidoExistente = pedidoDoBanco.ItensPedido
                                                               .FirstOrDefault(ip => ip.ProdutoId == produtoDoCatalogo.Id);

                        if (itemPedidoExistente == null)
                        {
                            var novoItemPedido = new ItemPedido(pedidoDoBanco.Id, produtoDoCatalogo.Id);
                            pedidoDoBanco.AddItemPedido(novoItemPedido);
                            response.ProdutosAdicionados++; 
                        }
                        else
                        {
                            response.ProdutosIgnorados++;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                Console.WriteLine($"Erro ao salvar no banco de dados: {ex.Message}");

                response.PedidosAdicionados = 0;
                response.ProdutosAdicionados = 0;
                response.PedidosIgnorados = 0;
                response.ProdutosIgnorados = 0;
            }

            return response;
        }
    }
}