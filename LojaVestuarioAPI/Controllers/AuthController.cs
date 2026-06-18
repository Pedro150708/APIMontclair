using BCrypt.Net;
using LojaVestuarioAPI.Data;
using LojaVestuarioAPI.DTOs;
using LojaVestuarioAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LojaVestuarioAPI.Services;
namespace LojaVestuarioAPI.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LojaContext _context;
    private readonly TokenService _tokenService;

    public AuthController(
        LojaContext context,
        TokenService tokenService)
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO dto)
    {
        var existe = await _context.Usuarios
            .AnyAsync(u => u.Email == dto.Email);

        if (existe)
        {
            return BadRequest("E-mail já cadastrado.");
        }

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Senha = BCrypt.Net.BCrypt.HashPassword(dto.Senha)
        };

        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensagem = "Usuário cadastrado com sucesso."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDTO dto)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Email == dto.Email);

        if (usuario == null)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }

        bool senhaValida =
            BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.Senha);

        if (!senhaValida)
        {
            return Unauthorized("Usuário ou senha inválidos.");
        }

        var token = _tokenService.GenerateToken(usuario);

        return Ok(new
        {
        token, usuario = new
        {
            usuario.Id,
            usuario.Nome,
            usuario.Email
        }
        });
    }

    [Authorize]
[HttpGet("me")]
public async Task<IActionResult> Me()
{
    var userId =
        User.FindFirst(
            ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
        return Unauthorized();
    }

    var usuario =
        await _context.Usuarios.FindAsync(
            int.Parse(userId));

    if (usuario == null)
    {
        return NotFound();
    }

    return Ok(new
    {
        usuario.Id,
        usuario.Nome,
        usuario.Email,
        usuario.Telefone,
        usuario.CriadoEm
    });
}

    [Authorize]
    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePassword(ChangePasswordDTO dto)
    {
        var userId =
            User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    
        if (userId == null)
        {
            return Unauthorized();
        }
    
        var usuario =
            await _context.Usuarios.FindAsync(
                int.Parse(userId));
    
        if (usuario == null)
        {
            return NotFound();
        }
    
        bool senhaCorreta =
            BCrypt.Net.BCrypt.Verify(
                dto.SenhaAtual,
                usuario.Senha);
    
        if (!senhaCorreta)
        {
            return BadRequest(
                "Senha atual incorreta.");
        }
    
        usuario.Senha =
            BCrypt.Net.BCrypt.HashPassword(
                dto.NovaSenha);
    
        await _context.SaveChangesAsync();
    
        return Ok(new
        {
            mensagem = "Senha alterada com sucesso."
        });
    }

    [Authorize]
[HttpPut("update-profile")]
public async Task<IActionResult> UpdateProfile(
    UpdateProfileDTO dto)
{
    var userId =
        User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

    if (userId == null)
    {
        return Unauthorized();
    }

    var usuario =
        await _context.Usuarios.FindAsync(
            int.Parse(userId));

    if (usuario == null)
    {
        return NotFound();
    }

    usuario.Nome = dto.Nome;
    usuario.Telefone = dto.Telefone;

    await _context.SaveChangesAsync();

    return Ok(new
    {
        mensagem = "Perfil atualizado com sucesso."
    });
}

[Authorize]
[HttpPost("logout")]
public IActionResult Logout()
{
    return Ok(new
    {
        mensagem = "Logout realizado com sucesso."
    });
}

    [HttpPost("forgot-password")]
public async Task<IActionResult> ForgotPassword(
    ForgotPasswordDTO dto)
{
    var usuario =
        await _context.Usuarios
            .FirstOrDefaultAsync(
                u => u.Email == dto.Email);

    if(usuario == null)
    {
        return NotFound(
            "Usuário não encontrado.");
    }

    return Ok(new
    {
        mensagem =
            "Solicitação recebida."
    });
}

[HttpPost("reset-password")]
public async Task<IActionResult> ResetPassword(
    ResetPasswordDTO dto)
{
    var usuario =
        await _context.Usuarios
            .FirstOrDefaultAsync(
                u => u.Email == dto.Email);

    if (usuario == null)
    {
        return NotFound(
            "Usuário não encontrado.");
    }

    usuario.Senha =
        BCrypt.Net.BCrypt.HashPassword(
            dto.NovaSenha);

    await _context.SaveChangesAsync();

    return Ok(new
    {
        mensagem =
            "Senha redefinida com sucesso."
    });
}
}