using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs;

public record PropostaResponse(
    Guid Id,
    string NomeCliente,
    string CpfCliente,
    string EmailCliente,
    TipoSeguro TipoSeguro,
    decimal ValorCobertura,
    decimal ValorPremio,
    StatusProposta Status,
    DateTime DataCriacao,
    DateTime? DataAtualizacao,
    string? Observacoes
);