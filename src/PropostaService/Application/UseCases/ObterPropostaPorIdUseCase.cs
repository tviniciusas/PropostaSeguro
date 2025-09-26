using PropostaService.Application.DTOs;
using PropostaService.Application.Ports;

namespace PropostaService.Application.UseCases;

public class ObterPropostaPorIdUseCase
{
    private readonly IPropostaRepository _propostaRepository;

    public ObterPropostaPorIdUseCase(IPropostaRepository propostaRepository)
    {
        _propostaRepository = propostaRepository;
    }

    public async Task<PropostaResponse?> ExecutarAsync(Guid id)
    {
        var proposta = await _propostaRepository.ObterPorIdAsync(id);

        if (proposta == null)
            return null;

        return new PropostaResponse(
            proposta.Id,
            proposta.NomeCliente,
            proposta.CpfCliente,
            proposta.EmailCliente,
            proposta.TipoSeguro,
            proposta.ValorCobertura,
            proposta.ValorPremio,
            proposta.Status,
            proposta.DataCriacao,
            proposta.DataAtualizacao,
            proposta.Observacoes
        );
    }
}