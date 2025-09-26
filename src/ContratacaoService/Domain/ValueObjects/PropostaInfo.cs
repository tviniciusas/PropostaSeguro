using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Domain.ValueObjects;

public record PropostaInfo(
    Guid Id,
    StatusProposta Status,
    string NomeCliente,
    string TipoSeguro,
    decimal ValorCobertura,
    decimal ValorPremio
);