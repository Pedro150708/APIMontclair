using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaVestuarioAPI.Data;
using LojaVestuarioAPI.Models;

namespace LojaVestuarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PedidosController : ControllerBase
{
    private readonly LojaContext _context;

    public PedidosController(LojaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Pedido>>> Get()
    {
        return await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.ItensPedido)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Pedido>> Get(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.Usuario)
            .Include(p => p.ItensPedido)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null)
            return NotFound();

        return pedido;
    }

    [HttpPost]
    public async Task<ActionResult<Pedido>> Post(Pedido pedido)
    {
        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get),
            new { id = pedido.Id },
            pedido);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Pedido pedido)
    {
        if (id != pedido.Id)
            return BadRequest();

        _context.Entry(pedido).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var pedido = await _context.Pedidos.FindAsync(id);

        if (pedido == null)
            return NotFound();

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}