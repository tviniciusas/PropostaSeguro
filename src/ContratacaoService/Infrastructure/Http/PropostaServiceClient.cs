using System.Text.Json;
using ContratacaoService.Application.Ports;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Infrastructure.Http;

public class PropostaServiceClient : IPropostaServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public PropostaServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<PropostaInfo?> ObterPropostaPorIdAsync(Guid propostaId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/propostas/{propostaId}");

            if (!response.IsSuccessStatusCode)
                return null;

            var content = await response.Content.ReadAsStringAsync();
            var propostaResponse = JsonSerializer.Deserialize<PropostaServiceResponse>(content, _jsonOptions);

            if (propostaResponse == null)
                return null;

            return new PropostaInfo(
                propostaResponse.Id,
                (StatusProposta)propostaResponse.Status,
                propostaResponse.NomeCliente,
                propostaResponse.TipoSeguro.ToString(),
                propostaResponse.ValorCobertura,
                propostaResponse.ValorPremio
            );
        }
        catch (Exception)
        {
            return null;
        }
    }

    private record PropostaServiceResponse(
        Guid Id,
        string NomeCliente,
        string CpfCliente,
        string EmailCliente,
        int TipoSeguro,
        decimal ValorCobertura,
        decimal ValorPremio,
        int Status,
        DateTime DataCriacao,
        DateTime? DataAtualizacao,
        string? Observacoes
    );
}