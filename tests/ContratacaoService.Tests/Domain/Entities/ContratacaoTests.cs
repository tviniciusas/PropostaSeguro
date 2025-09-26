using Xunit;
using ContratacaoService.Domain.Entities;

namespace ContratacaoService.Tests.Domain.Entities;

public class ContratacaoTests
{
    [Fact]
    public void Contratacao_ComParametrosValidos_DeveCriarInstancia()
    {
        var propostaId = Guid.NewGuid();
        var numeroApolice = "AP123456789";
        var dataInicio = DateTime.Today.AddDays(1);
        var dataFim = DateTime.Today.AddYears(1);

        var contratacao = new Contratacao(propostaId, numeroApolice, dataInicio, dataFim);

        Assert.NotEqual(Guid.Empty, contratacao.Id);
        Assert.Equal(propostaId, contratacao.PropostaId);
        Assert.Equal(numeroApolice, contratacao.NumeroApolice);
        Assert.Equal(dataInicio, contratacao.DataVigenciaInicio);
        Assert.Equal(dataFim, contratacao.DataVigenciaFim);
        Assert.True(contratacao.DataContratacao <= DateTime.UtcNow);
    }

    [Fact]
    public void Contratacao_ComPropostaIdVazio_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Contratacao(
            Guid.Empty,
            "AP123456789",
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        ));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Contratacao_ComNumeroApoliceInvalido_DeveLancarExcecao(string numeroInvalido)
    {
        Assert.Throws<ArgumentException>(() => new Contratacao(
            Guid.NewGuid(),
            numeroInvalido,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        ));
    }

    [Fact]
    public void Contratacao_ComDataInicioMaiorQueDataFim_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Contratacao(
            Guid.NewGuid(),
            "AP123456789",
            DateTime.Today.AddYears(1),
            DateTime.Today.AddDays(1)
        ));
    }

    [Fact]
    public void Contratacao_ComDataInicioNoPassado_DeveLancarExcecao()
    {
        Assert.Throws<ArgumentException>(() => new Contratacao(
            Guid.NewGuid(),
            "AP123456789",
            DateTime.Today.AddDays(-1),
            DateTime.Today.AddYears(1)
        ));
    }
}