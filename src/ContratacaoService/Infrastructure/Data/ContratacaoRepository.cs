using System.Data;
using Dapper;
using Npgsql;
using ContratacaoService.Application.Ports;
using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Infrastructure.Data;

public class ContratacaoRepository : IContratacaoRepository
{
    private readonly string _connectionString;

    public ContratacaoRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Contratacao?> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT id, propostaid, datacontratacao, numeroapolice, datavigenciainicio, datavigenciafim
            FROM contratacoes
            WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var row = await connection.QueryFirstOrDefaultAsync(sql, new { Id = id });

        return row != null ? MapearContratacao(row) : null;
    }

    public async Task<Contratacao?> ObterPorPropostaIdAsync(Guid propostaId)
    {
        const string sql = @"
            SELECT id, propostaid, datacontratacao, numeroapolice, datavigenciainicio, datavigenciafim
            FROM contratacoes
            WHERE propostaid = @PropostaId";

        using var connection = new NpgsqlConnection(_connectionString);
        var row = await connection.QueryFirstOrDefaultAsync(sql, new { PropostaId = propostaId });

        return row != null ? MapearContratacao(row) : null;
    }

    public async Task<IEnumerable<Contratacao>> ListarAsync()
    {
        const string sql = @"
            SELECT id, propostaid, datacontratacao, numeroapolice, datavigenciainicio, datavigenciafim
            FROM contratacoes
            ORDER BY datacontratacao DESC";

        using var connection = new NpgsqlConnection(_connectionString);
        var rows = await connection.QueryAsync(sql);

        return rows.Select(MapearContratacao);
    }

    public async Task<Guid> CriarAsync(Contratacao contratacao)
    {
        const string sql = @"
            INSERT INTO contratacoes (id, propostaid, datacontratacao, numeroapolice, datavigenciainicio, datavigenciafim)
            VALUES (@Id, @PropostaId, @DataContratacao, @NumeroApolice, @DataVigenciaInicio, @DataVigenciaFim)";

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            contratacao.Id,
            contratacao.PropostaId,
            contratacao.DataContratacao,
            contratacao.NumeroApolice,
            contratacao.DataVigenciaInicio,
            contratacao.DataVigenciaFim
        });

        return contratacao.Id;
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        const string sql = "SELECT COUNT(1) FROM contratacoes WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });

        return count > 0;
    }

    public async Task<bool> ExistePorPropostaIdAsync(Guid propostaId)
    {
        const string sql = "SELECT COUNT(1) FROM contratacoes WHERE propostaid = @PropostaId";

        using var connection = new NpgsqlConnection(_connectionString);
        var count = await connection.QuerySingleAsync<int>(sql, new { PropostaId = propostaId });

        return count > 0;
    }

    private static Contratacao MapearContratacao(dynamic row)
    {
        var contratacao = new Contratacao(
            (Guid)row.propostaid,
            (string)row.numeroapolice,
            (DateTime)row.datavigenciainicio,
            (DateTime)row.datavigenciafim
        );

        typeof(Contratacao).GetProperty("Id")?.SetValue(contratacao, (Guid)row.id);
        typeof(Contratacao).GetProperty("DataContratacao")?.SetValue(contratacao, (DateTime)row.datacontratacao);

        return contratacao;
    }
}