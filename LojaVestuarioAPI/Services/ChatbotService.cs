using LojaVestuarioAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace LojaVestuarioAPI.Services;

public class ChatbotService
{
    private readonly LojaContext _context;

    public ChatbotService(LojaContext context)
    {
        _context = context;
    }

    public async Task<string> Responder(string mensagem)
    {
        mensagem = mensagem.ToLower().Trim();

        // ==========================================
        // PRODUTOS PRETOS E ABAIXO DE R$100
        // ==========================================

        if ((mensagem.Contains("preto") || mensagem.Contains("pretos")) &&
            (mensagem.Contains("barato") ||
             mensagem.Contains("menos de 100") ||
             mensagem.Contains("abaixo de 100")))
        {
            var produtos = await _context.Produtos
                .Where(p =>
                    p.Cor != null &&
                    p.Cor.ToLower().Contains("preto") &&
                    p.Preco < 100)
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos pretos abaixo de R$100.";
            }

            string resposta =
                "Produtos pretos abaixo de R$100:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // RECOMENDAÇÕES ELEGANTES
        // ==========================================

        if (mensagem.Contains("elegante") ||
            mensagem.Contains("festa") ||
            mensagem.Contains("social"))
        {
            var produtos = await _context.Produtos
                .Where(p =>
                    p.Nome.Contains("Vestido") ||
                    p.Nome.Contains("Social"))
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos elegantes.";
            }

            string resposta =
                "Recomendo os seguintes produtos:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // RECOMENDAÇÕES PARA INVERNO
        // ==========================================

        if (mensagem.Contains("inverno") ||
            mensagem.Contains("frio"))
        {
            var produtos = await _context.Produtos
                .Where(p =>
                    p.Nome.Contains("Jaqueta") ||
                    p.Nome.Contains("Moletom"))
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos para o inverno.";
            }

            string resposta =
                "Produtos recomendados para o inverno:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // PRODUTOS PRETOS
        // ==========================================

        if (mensagem.Contains("preto"))
        {
            var produtos = await _context.Produtos
                .Where(p =>
                    p.Cor != null &&
                    p.Cor.ToLower().Contains("preto"))
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos pretos.";
            }

            string resposta =
                "Temos os seguintes produtos pretos:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // TAMANHO M
        // ==========================================

        if (mensagem.Contains("tamanho m"))
        {
            var produtos = await _context.Produtos
                .Where(p => p.Tamanho == "M")
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos tamanho M.";
            }

            string resposta =
                "Produtos tamanho M:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // PRODUTOS BARATOS
        // ==========================================

        if (mensagem.Contains("barato") ||
            mensagem.Contains("menos de 100") ||
            mensagem.Contains("abaixo de 100"))
        {
            var produtos = await _context.Produtos
                .Where(p => p.Preco < 100)
                .ToListAsync();

            if (!produtos.Any())
            {
                return "Não encontrei produtos abaixo de R$100.";
            }

            string resposta =
                "Produtos abaixo de R$100:\n\n";

            foreach (var produto in produtos)
            {
                resposta +=
                    $"• {produto.Nome} - R${produto.Preco:F2}\n";
            }

            return resposta;
        }

        // ==========================================
        // CONSULTA POR NOME DO PRODUTO
        // ==========================================

        var todosProdutos = await _context.Produtos.ToListAsync();

        foreach (var produto in todosProdutos)
        {
            if (mensagem.Contains(produto.Nome.ToLower()))
            {
                return
                    $"Produto: {produto.Nome}\n" +
                    $"Preço: R${produto.Preco:F2}\n" +
                    $"Estoque: {produto.Estoque}\n" +
                    $"Cor: {produto.Cor}\n" +
                    $"Tamanho: {produto.Tamanho}";
            }
        }

        // ==========================================
        // FRETE
        // ==========================================

        if (mensagem.Contains("frete"))
        {
            return
                "O valor do frete é calculado automaticamente durante a finalização da compra.";
        }

        // ==========================================
        // PAGAMENTO
        // ==========================================

        if (mensagem.Contains("pagamento") ||
            mensagem.Contains("pix") ||
            mensagem.Contains("cartão") ||
            mensagem.Contains("boleto"))
        {
            return
                "Aceitamos Pix, Cartão de Crédito, Cartão de Débito e Boleto Bancário.";
        }

        // ==========================================
        // TROCAS
        // ==========================================

        if (mensagem.Contains("troca") ||
            mensagem.Contains("devolução"))
        {
            return
                "O prazo para trocas e devoluções é de até 30 dias após o recebimento do produto.";
        }

        // ==========================================
        // AJUDA
        // ==========================================

        if (mensagem.Contains("ajuda") ||
            mensagem.Contains("opções"))
        {
            return
                "Posso ajudar com:\n\n" +
                "• Produtos pretos\n" +
                "• Produtos tamanho M\n" +
                "• Produtos abaixo de R$100\n" +
                "• Produtos elegantes\n" +
                "• Produtos para inverno\n" +
                "• Frete\n" +
                "• Pagamento\n" +
                "• Trocas\n\n" +
                "Exemplos:\n" +
                "\"Quais produtos pretos vocês têm?\"\n" +
                "\"Mostre produtos abaixo de R$100\"\n" +
                "\"Quero uma roupa elegante\"\n" +
                "\"Tem produtos para o inverno?\"";
        }

        // ==========================================
        // RESPOSTA PADRÃO
        // ==========================================

        return
            "Desculpe, não entendi sua pergunta.\n\n" +
            "Digite 'ajuda' para ver as opções disponíveis.";
    }
}