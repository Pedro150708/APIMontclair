using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaVestuarioAPI.Data;
using LojaVestuarioAPI.Models;

namespace LojaVestuarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItensPedidoController : ControllerBase
{
    private readonly LojaContext _context;

    public ItensPedidoController(LojaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ItemPedido>>> Get()
    {
        return await _context.ItensPedido
            .Include(i => i.Produto)
            .Include(i => i.Pedido)
            .ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemPedido>> Get(int id)
    {
        var item = await _context.ItensPedido
            .Include(i => i.Produto)
            .Include(i => i.Pedido)
            .FirstOrDefaultAsync(i => i.Id == id);

        if (item == null)
            return NotFound();

        return item;
    }

    [HttpPost]
    public async Task<ActionResult<ItemPedido>> Post(ItemPedido item)
    {
        _context.ItensPedido.Add(item);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get),
            new { id = item.Id },
            item);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, ItemPedido item)
    {
        if (id != item.Id)
            return BadRequest();

        _context.Entry(item).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await _context.ItensPedido.FindAsync(id);

        if (item == null)
            return NotFound();

        _context.ItensPedido.Remove(item);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}