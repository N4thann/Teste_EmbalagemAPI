using SrManoelLoja.DTOs.Request;
using SrManoelLoja.DTOs.Response;
using SrManoelLoja.Interfaces;
using SrManoelLoja.Entities;
using SrManoelLoja.ValueObjects;

namespace SrManoelLoja.Services
{
    public class EmbalagemService : IEmbalagemService
    {
        public async Task<PedidosResponse> ProcessarPedidos(PedidosRequest pedidosRequest)
        {
            var pedidosResponse = new PedidosResponse
            {
                Pedidos = new List<PedidoResponse>()
            };

            foreach (var pedidoRequest in pedidosRequest.Pedidos)
            {
                var pedidoResponse = new PedidoResponse
                {
                    Pedido_Id = pedidoRequest.Pedido_id,
                    Caixas = new List<CaixaResponse>()
                };

                var produtosNaoEmpacotados = pedidoRequest.Produtos
                    .Select(pr => new Produto(pr.Produto_id, new Dimensao(pr.Dimensoes.Altura, pr.Dimensoes.Largura, pr.Dimensoes.Comprimento)))
                    .ToList();

                produtosNaoEmpacotados = produtosNaoEmpacotados
                                        .OrderByDescending(p => p.Dimensoes.Altura * p.Dimensoes.Largura * p.Dimensoes.Comprimento)
                                        .ToList();

                var caixasDisponiveis = TipoDeCaixa.TiposDisponiveis
                                        .OrderBy(c => c.Dimensoes.Altura * c.Dimensoes.Largura * c.Dimensoes.Comprimento)
                                        .ToList();

                var caixasAlocadas = new List<Tuple<TipoDeCaixa, List<Produto>, decimal>>();

                while (produtosNaoEmpacotados.Any())
                {
                    bool produtoAlocado = false;
                    var produtoAtual = produtosNaoEmpacotados.First();
                    var produtoAtualVolume = produtoAtual.Dimensoes.Altura * produtoAtual.Dimensoes.Largura * produtoAtual.Dimensoes.Comprimento;

                    foreach (var caixaAlocada in caixasAlocadas)
                    {
                        var caixa = caixaAlocada.Item1;
                        var produtosNaCaixa = caixaAlocada.Item2;
                        var volumeOcupadoAtual = caixaAlocada.Item3;
                        var volumeMaxCaixa = caixa.Dimensoes.Altura * caixa.Dimensoes.Largura * caixa.Dimensoes.Comprimento;

                        if (PodeEncaixar(produtoAtual.Dimensoes, caixa.Dimensoes) && (volumeOcupadoAtual + produtoAtualVolume) <= volumeMaxCaixa)
                        {
                            produtosNaCaixa.Add(produtoAtual);
                            caixasAlocadas[caixasAlocadas.IndexOf(caixaAlocada)] = Tuple.Create(caixa, produtosNaCaixa, volumeOcupadoAtual + produtoAtualVolume);
                            produtosNaoEmpacotados.RemoveAt(0);
                            produtoAlocado = true;
                            break; 
                        }
                    }

                    if (!produtoAlocado)
                    {
                        TipoDeCaixa melhorCaixaParaProduto = null;
                        foreach (var tipoCaixa in caixasDisponiveis)
                        {
                            if (PodeEncaixar(produtoAtual.Dimensoes, tipoCaixa.Dimensoes))
                            {
                                melhorCaixaParaProduto = tipoCaixa;
                                break;
                            }
                        }

                        if (melhorCaixaParaProduto != null)
                        {
                            var novaListaDeProdutos = new List<Produto> { produtoAtual };
                            caixasAlocadas.Add(Tuple.Create(melhorCaixaParaProduto, novaListaDeProdutos, produtoAtualVolume));
                            produtosNaoEmpacotados.RemoveAt(0);
                            produtoAlocado = true;
                        }
                    }

                    if (!produtoAlocado && produtosNaoEmpacotados.Any())
                    {
                        pedidoResponse.Caixas.Add(new CaixaResponse
                        {
                            Caixa_Id = null,
                            Produtos = new List<string> { produtoAtual.Name },
                            Observacao = "Produto não cabe em nenhuma caixa disponível."
                        });
                        produtosNaoEmpacotados.RemoveAt(0); 
                    }
                    else if (!produtoAlocado && !produtosNaoEmpacotados.Any())
                    {
                        pedidoResponse.Caixas.Add(new CaixaResponse
                        {
                            Caixa_Id = null,
                            Produtos = new List<string> { produtoAtual.Name },
                            Observacao = "Produto não cabe em nenhuma caixa disponível."
                        });
                        produtosNaoEmpacotados.RemoveAt(0);
                    }
                }

                foreach (var caixaAlocada in caixasAlocadas)
                {
                    pedidoResponse.Caixas.Add(new CaixaResponse
                    {
                        Caixa_Id = caixaAlocada.Item1.Nome,
                        Produtos = caixaAlocada.Item2.Select(p => p.Name).ToList()
                    });
                }

                if (produtosNaoEmpacotados.Any()) 
                {
                    foreach (var produtoNaoEmpacotado in produtosNaoEmpacotados)
                    {
                        pedidoResponse.Caixas.Add(new CaixaResponse
                        {
                            Caixa_Id = null,
                            Produtos = new List<string> { produtoNaoEmpacotado.Name },
                            Observacao = "Produto não cabe em nenhuma caixa disponível (loop final)."
                        });
                    }
                }

                pedidosResponse.Pedidos.Add(pedidoResponse);
            }

            return pedidosResponse;
        }

        private bool PodeEncaixar(Dimensao produtoDimensao, Dimensao caixaDimensao)
        {
            var p = new decimal[] { produtoDimensao.Altura, produtoDimensao.Largura, produtoDimensao.Comprimento };
            var c = new decimal[] { caixaDimensao.Altura, caixaDimensao.Largura, caixaDimensao.Comprimento };

            // Tenta todas as 6 orientações possíveis do produto dentro da caixa
            // (p[0], p[1], p[2]) - A, L, C
            if (p[0] <= c[0] && p[1] <= c[1] && p[2] <= c[2]) return true;
            // (p[0], p[2], p[1]) - A, C, L
            if (p[0] <= c[0] && p[2] <= c[1] && p[1] <= c[2]) return true;
            // (p[1], p[0], p[2]) - L, A, C
            if (p[1] <= c[0] && p[0] <= c[1] && p[2] <= c[2]) return true;
            // (p[1], p[2], p[0]) - L, C, A
            if (p[1] <= c[0] && p[2] <= c[1] && p[0] <= c[2]) return true;
            // (p[2], p[0], p[1]) - C, A, L
            if (p[2] <= c[0] && p[0] <= c[1] && p[1] <= c[2]) return true;
            // (p[2], p[1], p[0]) - C, L, A
            if (p[2] <= c[0] && p[1] <= c[1] && p[0] <= c[2]) return true;

            return false;
        }
    }
}