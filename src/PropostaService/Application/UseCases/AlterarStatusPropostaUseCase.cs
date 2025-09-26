using PropostaService.Application.DTOs;
using PropostaService.Application.Ports;

namespace PropostaService.Application.UseCases;

public class AlterarStatusPropostaUseCase
{
    private readonly IPropostaRepository _propostaRepository;

    public AlterarStatusPropostaUseCase(IPropostaRepository propostaRepository)
    {
        _propostaRepository = propostaRepository;
    }

    public async Task<PropostaResponse> ExecutarAsync(AlterarStatusPropostaRequest request)
    {
        var proposta = await _propostaRepository.ObterPorIdAsync(request.PropostaId);

        if (proposta == null)
            throw new ArgumentException($"Proposta com ID {request.PropostaId} não encontrada");

        proposta.AlterarStatus(request.NovoStatus, request.Observacoes);

        await _propostaRepository.AtualizarAsync(proposta);

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