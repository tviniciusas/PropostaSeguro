using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Ports;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;

namespace ContratacaoService.Application.UseCases;

public class CriarContratacaoUseCase
{
    private readonly IContratacaoRepository _contratacaoRepository;
    private readonly IPropostaServiceClient _propostaServiceClient;

    public CriarContratacaoUseCase(
        IContratacaoRepository contratacaoRepository,
        IPropostaServiceClient propostaServiceClient)
    {
        _contratacaoRepository = contratacaoRepository;
        _propostaServiceClient = propostaServiceClient;
    }

    public async Task<ContratacaoResponse> ExecutarAsync(CriarContratacaoRequest request)
    {
        var proposta = await _propostaServiceClient.ObterPropostaPorIdAsync(request.PropostaId);

        if (proposta == null)
            throw new ArgumentException($"Proposta com ID {request.PropostaId} não encontrada");

        if (proposta.Status != StatusProposta.Aprovada)
            throw new ArgumentException("Apenas propostas aprovadas podem ser contratadas");

        var contratacaoExistente = await _contratacaoRepository.ExistePorPropostaIdAsync(request.PropostaId);
        if (contratacaoExistente)
            throw new ArgumentException("Esta proposta já foi contratada");

        var numeroApolice = GerarNumeroApolice();

        var contratacao = new Contratacao(
            request.PropostaId,
            numeroApolice,
            request.DataVigenciaInicio,
            request.DataVigenciaFim
        );

        await _contratacaoRepository.CriarAsync(contratacao);

        return new ContratacaoResponse(
            contratacao.Id,
            contratacao.PropostaId,
            contratacao.DataContratacao,
            contratacao.NumeroApolice,
            contratacao.DataVigenciaInicio,
            contratacao.DataVigenciaFim
        );
    }

    private static string GerarNumeroApolice()
    {
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"AP{timestamp}{random}";
    }
}