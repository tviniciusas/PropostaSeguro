using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Application.Ports;

public interface IPropostaServiceClient
{
    Task<PropostaInfo?> ObterPropostaPorIdAsync(Guid propostaId);
}