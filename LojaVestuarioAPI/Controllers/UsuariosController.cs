using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaVestuarioAPI.Data;
using LojaVestuarioAPI.Models;

namespace LojaVestuarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsuariosController : ControllerBase
{
    private readonly LojaContext _context;

    public UsuariosController(LojaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var usuarios = await _context.Usuarios.Select(u => new
        {
            u.Id,
            u.Nome,
            u.Email,
            u.Telefone,
            u.CriadoEm
        })
        .ToListAsync();

    return Ok(usuarios);
}

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var usuario = await _context.Usuarios.Where(u => u.Id == id).Select(u => new
        {
            u.Id,
            u.Nome,
            u.Email,
            u.Telefone,
            u.CriadoEm
        })
        .FirstOrDefaultAsync();
        if (usuario == null)
            return NotFound();
            return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> Post(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(Get),
            new { id = usuario.Id },
            usuario);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, Usuario usuario)
    {
        if (id != usuario.Id)
            return BadRequest();

        _context.Entry(usuario).State = EntityState.Modified;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
            return NotFound();

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}