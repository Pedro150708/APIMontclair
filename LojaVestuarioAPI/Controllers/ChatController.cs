using LojaVestuarioAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace LojaVestuarioAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly ChatbotService _chatbot;

    public ChatController(ChatbotService chatbot)
    {
        _chatbot = chatbot;
    }

    [HttpPost]
    public async Task<IActionResult> Chat(ChatRequest request)
    {
        var resposta =
            await _chatbot.Responder(request.Mensagem);

        return Ok(new
        {
            resposta
        });
    }
}

public class ChatRequest
{
    public string Mensagem { get; set; } = "";
}