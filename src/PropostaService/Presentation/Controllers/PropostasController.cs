using Microsoft.AspNetCore.Mvc;
using PropostaService.Application.DTOs;
using PropostaService.Application.UseCases;
using PropostaService.Domain.Enums;

namespace PropostaService.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropostasController : ControllerBase
{
    private readonly CriarPropostaUseCase _criarPropostaUseCase;
    private readonly ListarPropostasUseCase _listarPropostasUseCase;
    private readonly AlterarStatusPropostaUseCase _alterarStatusPropostaUseCase;
    private readonly ObterPropostaPorIdUseCase _obterPropostaPorIdUseCase;

    public PropostasController(
        CriarPropostaUseCase criarPropostaUseCase,
        ListarPropostasUseCase listarPropostasUseCase,
        AlterarStatusPropostaUseCase alterarStatusPropostaUseCase,
        ObterPropostaPorIdUseCase obterPropostaPorIdUseCase)
    {
        _criarPropostaUseCase = criarPropostaUseCase;
        _listarPropostasUseCase = listarPropostasUseCase;
        _alterarStatusPropostaUseCase = alterarStatusPropostaUseCase;
        _obterPropostaPorIdUseCase = obterPropostaPorIdUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<PropostaResponse>> CriarProposta([FromBody] CriarPropostaRequest request)
    {
        try
        {
            var resultado = await _criarPropostaUseCase.ExecutarAsync(request);
            return CreatedAtAction(nameof(ObterPorId), new { id = resultado.Id }, resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PropostaResponse>>> Listar([FromQuery] StatusProposta? status = null)
    {
        var resultado = status.HasValue
            ? await _listarPropostasUseCase.ExecutarPorStatusAsync(status.Value)
            : await _listarPropostasUseCase.ExecutarAsync();

        return Ok(resultado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<PropostaResponse>> ObterPorId(Guid id)
    {
        var resultado = await _obterPropostaPorIdUseCase.ExecutarAsync(id);

        if (resultado == null)
            return NotFound(new { message = $"Proposta com ID {id} não encontrada" });

        return Ok(resultado);
    }

    [HttpPut("{id}/status")]
    public async Task<ActionResult<PropostaResponse>> AlterarStatus(
        Guid id,
        [FromBody] AlterarStatusRequest request)
    {
        try
        {
            var alterarStatusRequest = new AlterarStatusPropostaRequest(
                id,
                request.NovoStatus,
                request.Observacoes);

            var resultado = await _alterarStatusPropostaUseCase.ExecutarAsync(alterarStatusRequest);
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}

public record AlterarStatusRequest(StatusProposta NovoStatus, string? Observacoes = null);