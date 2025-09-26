using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Application.Ports;

public interface IContratacaoRepository
{
    Task<Contratacao?> ObterPorIdAsync(Guid id);
    Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId);
    Task<IEnumerable<Contratacao>> ListarAsync();
    Task<Guid> CriarAsync(Contratacao contratacao);
    Task<bool> ExisteAsync(Guid id);
    Task<bool> ExistePorPropostaIdAsync(Guid propostaId);
}