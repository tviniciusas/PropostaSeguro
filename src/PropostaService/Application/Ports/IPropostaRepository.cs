using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;

namespace PropostaService.Application.Ports;

public interface IPropostaRepository
{
    Task<Proposta?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Proposta>> ListarAsync();
    Task<IEnumerable<Proposta>> ListarPorStatusAsync(StatusProposta status);
    Task<IEnumerable<Proposta>> ListarPorCpfAsync(string cpf);
    Task<Guid> CriarAsync(Proposta proposta);
    Task AtualizarAsync(Proposta proposta);
    Task<bool> ExisteAsync(Guid id);
}