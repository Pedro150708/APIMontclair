using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVestuarioAPI.Models;

[Table("produtos")]
public class Produto
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("descricao")]
    public string? Descricao { get; set; }

    [Column("preco")]
    public decimal Preco { get; set; }

    [Column("estoque")]
    public int Estoque { get; set; }

    [Column("tamanho")]
    public string? Tamanho { get; set; }

    [Column("cor")]
    public string? Cor { get; set; }

    [Column("imagem")]
    public string? Imagem { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }

    public ICollection<ItemPedido>? ItensPedido { get; set; }
}