using System.Data;
using Dapper;
using Npgsql;
using PropostaService.Application.Ports;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Infrastructure.Data;

public class PropostaRepository : IPropostaRepository
{
    private readonly string _connectionString;

    public PropostaRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<Proposta?> ObterPorIdAsync(Guid id)
    {
        const string sql = @"
            SELECT id, nomecliente, cpfcliente, emailcliente, tiposeguro, valorcobertura,
                   valorpremio, status, datacriacao, dataatualizacao, observacoes
            FROM propostas
            WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var row = await connection.QueryFirstOrDefaultAsync(sql, new { Id = id });

        return row != null ? MapearProposta(row) : null;
    }

    public async Task<IEnumerable<Proposta>> ListarAsync()
    {
        const string sql = @"
            SELECT id, nomecliente, cpfcliente, emailcliente, tiposeguro, valorcobertura,
                   valorpremio, status, datacriacao, dataatualizacao, observacoes
            FROM propostas
            ORDER BY datacriacao DESC";

        using var connection = new NpgsqlConnection(_connectionString);
        var rows = await connection.QueryAsync(sql);

        return rows.Select(MapearProposta);
    }

    public async Task<IEnumerable<Proposta>> ListarPorStatusAsync(StatusProposta status)
    {
        const string sql = @"
            SELECT id, nomecliente, cpfcliente, emailcliente, tiposeguro, valorcobertura,
                   valorpremio, status, datacriacao, dataatualizacao, observacoes
            FROM propostas
            WHERE status = @Status
            ORDER BY datacriacao DESC";

        using var connection = new NpgsqlConnection(_connectionString);
        var rows = await connection.QueryAsync(sql, new { Status = (int)status });

        return rows.Select(MapearProposta);
    }

    public async Task<IEnumerable<Proposta>> ListarPorCpfAsync(string cpf)
    {
        const string sql = @"
            SELECT id, nomecliente, cpfcliente, emailcliente, tiposeguro, valorcobertura,
                   valorpremio, status, datacriacao, dataatualizacao, observacoes
            FROM propostas
            WHERE cpfcliente = @Cpf
            ORDER BY datacriacao DESC";

        using var connection = new NpgsqlConnection(_connectionString);
        var rows = await connection.QueryAsync(sql, new { Cpf = cpf });

        return rows.Select(MapearProposta);
    }

    public async Task<Guid> CriarAsync(Proposta proposta)
    {
        const string sql = @"
            INSERT INTO propostas (id, nomecliente, cpfcliente, emailcliente, tiposeguro,
                                   valorcobertura, valorpremio, status, datacriacao,
                                   dataatualizacao, observacoes)
            VALUES (@Id, @NomeCliente, @CpfCliente, @EmailCliente, @TipoSeguro,
                    @ValorCobertura, @ValorPremio, @Status, @DataCriacao,
                    @DataAtualizacao, @Observacoes)";

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            proposta.Id,
            proposta.NomeCliente,
            CpfCliente = proposta.CpfCliente.Valor,
            EmailCliente = proposta.EmailCliente.Valor,
            TipoSeguro = (int)proposta.TipoSeguro,
            ValorCobertura = proposta.ValorCobertura.Valor,
            ValorPremio = proposta.ValorPremio.Valor,
            Status = (int)proposta.Status,
            proposta.DataCriacao,
            proposta.DataAtualizacao,
            proposta.Observacoes
        });

        return proposta.Id;
    }

    public async Task AtualizarAsync(Proposta proposta)
    {
        const string sql = @"
            UPDATE propostas
            SET status = @Status, dataatualizacao = @DataAtualizacao, observacoes = @Observacoes
            WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        await connection.ExecuteAsync(sql, new
        {
            proposta.Id,
            Status = (int)proposta.Status,
            proposta.DataAtualizacao,
            proposta.Observacoes
        });
    }

    public async Task<bool> ExisteAsync(Guid id)
    {
        const string sql = "SELECT COUNT(1) FROM propostas WHERE id = @Id";

        using var connection = new NpgsqlConnection(_connectionString);
        var count = await connection.QuerySingleAsync<int>(sql, new { Id = id });

        return count > 0;
    }

    private static Proposta MapearProposta(dynamic row)
    {
        var cpf = new CPF((string)row.cpfcliente);
        var email = new Email((string)row.emailcliente);
        var valorCobertura = new Dinheiro((decimal)row.valorcobertura);
        var valorPremio = new Dinheiro((decimal)row.valorpremio);

        var proposta = new Proposta(
            (string)row.nomecliente,
            cpf,
            email,
            (TipoSeguro)row.tiposeguro,
            valorCobertura,
            valorPremio,
            row.observacoes
        );

        typeof(Proposta).GetProperty("Id")?.SetValue(proposta, (Guid)row.id);
        typeof(Proposta).GetProperty("DataCriacao")?.SetValue(proposta, (DateTime)row.datacriacao);

        if (row.dataatualizacao != null)
        {
            typeof(Proposta).GetProperty("DataAtualizacao")?.SetValue(proposta, (DateTime?)row.dataatualizacao);
        }

        if ((int)row.status != (int)StatusProposta.EmAnalise)
        {
            proposta.AlterarStatus((StatusProposta)row.status, row.observacoes);
        }

        return proposta;
    }
}