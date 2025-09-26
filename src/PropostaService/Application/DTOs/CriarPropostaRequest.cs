using PropostaService.Domain.Enums;

namespace PropostaService.Application.DTOs;

public record CriarPropostaRequest(
    string NomeCliente,
    string CpfCliente,
    string EmailCliente,
    TipoSeguro TipoSeguro,
    decimal ValorCobertura,
    decimal ValorPremio,
    string? Observacoes = null
);