using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVestuarioAPI.Models;

[Table("itens_pedido")]
public class ItemPedido
{
    [Column("id")]
    public int Id { get; set; }

    [Column("pedido_id")]
    public int PedidoId { get; set; }

    [Column("produto_id")]
    public int ProdutoId { get; set; }

    [Column("quantidade")]
    public int Quantidade { get; set; }

    [Column("preco_unitario")]
    public decimal PrecoUnitario { get; set; }

    public Pedido? Pedido { get; set; }

    public Produto? Produto { get; set; }
}