using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs;

public record AlterarStatusPropostaRequest(
    Guid PropostaId,
    StatusProposta NovoStatus,
    string? Observacoes = null
);