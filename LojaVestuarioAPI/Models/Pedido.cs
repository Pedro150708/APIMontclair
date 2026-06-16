using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVestuarioAPI.Models;

[Table("pedidos")]
public class Pedido
{
    [Column("id")]
    public int Id { get; set; }

    [Column("usuario_id")]
    public int UsuarioId { get; set; }

    [Column("valor_total")]
    public decimal ValorTotal { get; set; }

    [Column("status_pedido")]
    public string StatusPedido { get; set; } = "Pendente";

    [Column("endereco_entrega")]
    public string? EnderecoEntrega { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }

    public Usuario? Usuario { get; set; }

    public ICollection<ItemPedido>? ItensPedido { get; set; }
}