namespace ContratacaoService.Application.DTOs;

public record CriarContratacaoRequest(
    Guid PropostaId,
    DateTime DataVigenciaInicio,
    DateTime DataVigenciaFim
);