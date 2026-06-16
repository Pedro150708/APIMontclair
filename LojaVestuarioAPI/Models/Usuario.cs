using System.ComponentModel.DataAnnotations.Schema;

namespace LojaVestuarioAPI.Models;

[Table("usuarios")]
public class Usuario
{
    [Column("id")]
    public int Id { get; set; }

    [Column("nome")]
    public string Nome { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("senha")]
    public string Senha { get; set; } = string.Empty;

    [Column("telefone")]
    public string? Telefone { get; set; }

    [Column("criado_em")]
    public DateTime CriadoEm { get; set; }

    public ICollection<Pedido>? Pedidos { get; set; }
}