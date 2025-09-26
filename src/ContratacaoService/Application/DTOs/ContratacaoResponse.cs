namespace ContratacaoService.Application.DTOs;

public record ContratacaoResponse(
    Guid Id,
    Guid PropostaId,
    DateTime DataContratacao,
    string NumeroApolice,
    DateTime DataVigenciaInicio,
    DateTime DataVigenciaFim
);