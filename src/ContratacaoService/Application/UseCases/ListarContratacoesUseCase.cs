using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Ports;

namespace ContratacaoService.Application.UseCases;

public class ListarContratacoesUseCase
{
    private readonly IContratacaoRepository _contratacaoRepository;

    public ListarContratacoesUseCase(IContratacaoRepository contratacaoRepository)
    {
        _contratacaoRepository = contratacaoRepository;
    }

    public async Task<IEnumerable<ContratacaoResponse>> ExecutarAsync()
    {
        var contratacoes = await _contratacaoRepository.ListarAsync();

        return contratacoes.Select(c => new ContratacaoResponse(
            c.Id,
            c.PropostaId,
            c.DataContratacao,
            c.NumeroApolice,
            c.DataVigenciaInicio,
            c.DataVigenciaFim
        ));
    }
}