using Microsoft.EntityFrameworkCore;
using LojaVestuarioAPI.Models;

namespace LojaVestuarioAPI.Data;

public class LojaContext : DbContext
{
    public LojaContext(DbContextOptions<LojaContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Tabelas
        modelBuilder.Entity<Usuario>().ToTable("usuarios");
        modelBuilder.Entity<Produto>().ToTable("produtos");
        modelBuilder.Entity<Pedido>().ToTable("pedidos");
        modelBuilder.Entity<ItemPedido>().ToTable("itens_pedido");

        modelBuilder.Entity<Produto>()
            .Property(p => p.CriadoEm)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<Usuario>()
            .Property(u => u.CriadoEm)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");
        
        modelBuilder.Entity<Pedido>()
            .Property(p => p.CriadoEm)
            .HasDefaultValueSql("CURRENT_TIMESTAMP");

        // Pedido -> Usuario
        modelBuilder.Entity<Pedido>()
            .HasOne(p => p.Usuario)
            .WithMany(u => u.Pedidos)
            .HasForeignKey(p => p.UsuarioId);

        // ItemPedido -> Pedido
        modelBuilder.Entity<ItemPedido>()
            .HasOne(i => i.Pedido)
            .WithMany(p => p.ItensPedido)
            .HasForeignKey(i => i.PedidoId);

        // ItemPedido -> Produto
        modelBuilder.Entity<ItemPedido>()
            .HasOne(i => i.Produto)
            .WithMany(p => p.ItensPedido)
            .HasForeignKey(i => i.ProdutoId);

        base.OnModelCreating(modelBuilder);
    }
}