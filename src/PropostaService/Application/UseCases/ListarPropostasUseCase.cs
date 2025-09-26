using PropostaService.Application.DTOs;
using PropostaService.Application.Ports;
using PropostaService.Domain.Enums;

namespace PropostaService.Application.UseCases;

public class ListarPropostasUseCase
{
    private readonly IPropostaRepository _propostaRepository;

    public ListarPropostasUseCase(IPropostaRepository propostaRepository)
    {
        _propostaRepository = propostaRepository;
    }

    public async Task<IEnumerable<PropostaResponse>> ExecutarAsync()
    {
        var propostas = await _propostaRepository.ListarAsync();

        return propostas.Select(p => new PropostaResponse(
            p.Id,
            p.NomeCliente,
            p.CpfCliente,
            p.EmailCliente,
            p.TipoSeguro,
            p.ValorCobertura,
            p.ValorPremio,
            p.Status,
            p.DataCriacao,
            p.DataAtualizacao,
            p.Observacoes
        ));
    }

    public async Task<IEnumerable<PropostaResponse>> ExecutarPorStatusAsync(StatusProposta status)
    {
        var propostas = await _propostaRepository.ListarPorStatusAsync(status);

        return propostas.Select(p => new PropostaResponse(
            p.Id,
            p.NomeCliente,
            p.CpfCliente,
            p.EmailCliente,
            p.TipoSeguro,
            p.ValorCobertura,
            p.ValorPremio,
            p.Status,
            p.DataCriacao,
            p.DataAtualizacao,
            p.Observacoes
        ));
    }
}