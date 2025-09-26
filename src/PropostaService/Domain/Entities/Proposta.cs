using PropostaService.Domain.Enums;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Domain.Entities;

public class Proposta
{
    public Guid Id { get; private set; }
    public string NomeCliente { get; private set; }
    public CPF CpfCliente { get; private set; }
    public Email EmailCliente { get; private set; }
    public TipoSeguro TipoSeguro { get; private set; }
    public Dinheiro ValorCobertura { get; private set; }
    public Dinheiro ValorPremio { get; private set; }
    public StatusProposta Status { get; private set; }
    public DateTime DataCriacao { get; private set; }
    public DateTime? DataAtualizacao { get; private set; }
    public string? Observacoes { get; private set; }

    private Proposta() { }

    public Proposta(
        string nomeCliente,
        CPF cpfCliente,
        Email emailCliente,
        TipoSeguro tipoSeguro,
        Dinheiro valorCobertura,
        Dinheiro valorPremio,
        string? observacoes = null)
    {
        ValidarParametros(nomeCliente, valorCobertura, valorPremio);

        Id = Guid.NewGuid();
        NomeCliente = nomeCliente.Trim();
        CpfCliente = cpfCliente;
        EmailCliente = emailCliente;
        TipoSeguro = tipoSeguro;
        ValorCobertura = valorCobertura;
        ValorPremio = valorPremio;
        Status = StatusProposta.EmAnalise;
        DataCriacao = DateTime.UtcNow;
        Observacoes = observacoes?.Trim();
    }

    public void AlterarStatus(StatusProposta novoStatus, string? observacoes = null)
    {
        if (Status == novoStatus)
            return;

        Status = novoStatus;
        DataAtualizacao = DateTime.UtcNow;

        if (!string.IsNullOrWhiteSpace(observacoes))
            Observacoes = observacoes.Trim();
    }

    public bool PodeSerContratada() => Status == StatusProposta.Aprovada;

    public void Aprovar(string? observacoes = null)
    {
        AlterarStatus(StatusProposta.Aprovada, observacoes);
    }

    public void Rejeitar(string? observacoes = null)
    {
        if (string.IsNullOrWhiteSpace(observacoes))
            throw new ArgumentException("Observações são obrigatórias para rejeição", nameof(observacoes));

        AlterarStatus(StatusProposta.Rejeitada, observacoes);
    }

    private static void ValidarParametros(string nomeCliente, Dinheiro valorCobertura, Dinheiro valorPremio)
    {
        if (string.IsNullOrWhiteSpace(nomeCliente))
            throw new ArgumentException("Nome do cliente é obrigatório", nameof(nomeCliente));

        if (valorCobertura.Valor <= 0)
            throw new ArgumentException("Valor de cobertura deve ser maior que zero", nameof(valorCobertura));

        if (valorPremio.Valor <= 0)
            throw new ArgumentException("Valor do prêmio deve ser maior que zero", nameof(valorPremio));

        if (valorPremio >= valorCobertura)
            throw new ArgumentException("Valor do prêmio deve ser menor que o valor de cobertura", nameof(valorPremio));
    }
}