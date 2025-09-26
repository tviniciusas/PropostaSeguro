using Microsoft.AspNetCore.Mvc;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.UseCases;

namespace ContratacaoService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContratacoesController : ControllerBase
{
    private readonly CriarContratacaoUseCase _criarContratacaoUseCase;
    private readonly ListarContratacoesUseCase _listarContratacoesUseCase;

    public ContratacoesController(
        CriarContratacaoUseCase criarContratacaoUseCase,
        ListarContratacoesUseCase listarContratacoesUseCase)
    {
        _criarContratacaoUseCase = criarContratacaoUseCase;
        _listarContratacoesUseCase = listarContratacoesUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<ContratacaoResponse>> CriarContratacao([FromBody] CriarContratacaoRequest request)
    {
        try
        {
            var resultado = await _criarContratacaoUseCase.ExecutarAsync(request);
            return CreatedAtAction(nameof(CriarContratacao), new { id = resultado.Id }, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ContratacaoResponse>>> Listar()
    {
        var resultado = await _listarContratacoesUseCase.ExecutarAsync();
        return Ok(resultado);
    }
}