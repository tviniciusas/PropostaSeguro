using Xunit;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Tests.Domain.Entities;

public class PropostaTests
{
    private readonly CPF _cpfValido = new("12345678901");
    private readonly Email _emailValido = new("teste@exemplo.com");
    private readonly Dinheiro _valorCobertura = new(100000);
    private readonly Dinheiro _valorPremio = new(5000);

    [Fact]
    public void Proposta_ComParametrosValidos_DeveCriarInstancia()
    {
        var proposta = new Proposta(
            "João Silva",
            _cpfValido,
            _emailValido,
            TipoSeguro.Vida,
            _valorCobertura,
            _valorPremio
        );

        Assert.NotEqual(Guid.Empty, proposta.Id);
        Assert.Equal("João Silva", proposta.NomeCliente);
        Assert.Equal(_cpfValido, proposta.CpfCliente);
        Assert.Equal(_emailValido, proposta.EmailCliente);
        Assert.Equal(TipoSeguro.Vida, proposta.TipoSeguro);
        Assert.Equal(_valorCobertura, proposta.ValorCobertura);
        Assert.Equal(_valorPremio, proposta.ValorPremio);
        Assert.Equal(StatusProposta.EmAnalise, proposta.Status);
        Assert.True(proposta.DataCriacao <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Proposta_ComNomeClienteInvalido_DeveLancarExcecao(string nomeInvalido)
    {
        Assert.Throws<ArgumentException>(() => new Proposta(
            nomeInvalido,
            _cpfValido,
            _emailValido,
            TipoSeguro.Vida,
            _valorCobertura,
            _valorPremio
        ));
    }

    [Fact]
    public void Proposta_ComValorCoberturaZero_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Proposta(
            "João Silva",
            _cpfValido,
            _emailValido,
            TipoSeguro.Vida,
            new Dinheiro(0),
            _valorPremio
        ));
    }

    [Fact]
    public void Proposta_ComValorPremioMaiorQueCobertura_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Proposta(
            "João Silva",
            _cpfValido,
            _emailValido,
            TipoSeguro.Vida,
            new Dinheiro(5000),
            new Dinheiro(10000)
        ));
    }

    [Fact]
    public void AlterarStatus_ComNovoStatus_DeveAtualizarStatus()
    {
        var proposta = CriarPropostaValida();

        proposta.AlterarStatus(StatusProposta.Aprovada, "Proposta aprovada");

        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        Assert.Equal("Proposta aprovada", proposta.Observacoes);
        Assert.NotNull(proposta.DataAtualizacao);
    }

    [Fact]
    public void Aprovar_DeveMudarStatusParaAprovada()
    {
        var proposta = CriarPropostaValida();

        proposta.Aprovar("Documentação completa");

        Assert.Equal(StatusProposta.Aprovada, proposta.Status);
        Assert.Equal("Documentação completa", proposta.Observacoes);
    }

    [Fact]
    public void Rejeitar_SemObservacoes_DeveLancarExcecao()
    {
        var proposta = CriarPropostaValida();

        Assert.Throws<ArgumentException>(() => proposta.Rejeitar(""));
    }

    [Fact]
    public void PodeSerContratada_ComStatusAprovada_DeveRetornarTrue()
    {
        var proposta = CriarPropostaValida();
        proposta.Aprovar();

        Assert.True(proposta.PodeSerContratada());
    }

    [Fact]
    public void PodeSerContratada_ComStatusEmAnalise_DeveRetornarFalse()
    {
        var proposta = CriarPropostaValida();

        Assert.False(proposta.PodeSerContratada());
    }

    private Proposta CriarPropostaValida()
    {
        return new Proposta(
            "João Silva",
            _cpfValido,
            _emailValido,
            TipoSeguro.Vida,
            _valorCobertura,
            _valorPremio
        );
    }
}