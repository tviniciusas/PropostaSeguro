using PropostaService.Application.DTOs;
using PropostaService.Application.Ports;
using PropostaService.Domain.Entities;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Application.UseCases;

public class CriarPropostaUseCase
{
    private readonly IPropostaRepository _propostaRepository;

    public CriarPropostaUseCase(IPropostaRepository propostaRepository)
    {
        _propostaRepository = propostaRepository;
    }

    public async Task<PropostaResponse> ExecutarAsync(CriarPropostaRequest request)
    {
        var cpf = new CPF(request.CpfCliente);
        var email = new Email(request.EmailCliente);
        var valorCobertura = new Dinheiro(request.ValorCobertura);
        var valorPremio = new Dinheiro(request.ValorPremio);

        var proposta = new Proposta(
            request.NomeCliente,
            cpf,
            email,
            request.TipoSeguro,
            valorCobertura,
            valorPremio,
            request.Observacoes
        );

        await _propostaRepository.CriarAsync(proposta);

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