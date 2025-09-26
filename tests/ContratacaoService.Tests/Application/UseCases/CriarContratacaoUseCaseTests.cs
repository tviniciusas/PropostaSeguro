using Xunit;
using Moq;
using ContratacaoService.Application.DTOs;
using ContratacaoService.Application.Ports;
using ContratacaoService.Application.UseCases;
using ContratacaoService.Domain.Entities;
using ContratacaoService.Domain.Enums;
using ContratacaoService.Domain.ValueObjects;

namespace ContratacaoService.Tests.Application.UseCases;

public class CriarContratacaoUseCaseTests
{
    private readonly Mock<IContratacaoRepository> _contratacaoRepositoryMock;
    private readonly Mock<IPropostaServiceClient> _propostaServiceClientMock;
    private readonly CriarContratacaoUseCase _useCase;

    public CriarContratacaoUseCaseTests()
    {
        _contratacaoRepositoryMock = new Mock<IContratacaoRepository>();
        _propostaServiceClientMock = new Mock<IPropostaServiceClient>();
        _useCase = new CriarContratacaoUseCase(_contratacaoRepositoryMock.Object, _propostaServiceClientMock.Object);
    }

    [Fact]
    public async Task ExecutarAsync_ComPropostaAprovada_DeveCriarContratacao()
    {
        var propostaId = Guid.NewGuid();
        var request = new CriarContratacaoRequest(
            propostaId,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        );

        var propostaInfo = new PropostaInfo(
            propostaId,
            StatusProposta.Aprovada,
            "João Silva",
            "Vida",
            100000m,
            5000m
        );

        _propostaServiceClientMock
            .Setup(c => c.ObterPropostaPorIdAsync(propostaId))
            .ReturnsAsync(propostaInfo);

        _contratacaoRepositoryMock
            .Setup(r => r.ExistePorPropostaIdAsync(propostaId))
            .ReturnsAsync(false);

        _contratacaoRepositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Contratacao>()))
            .ReturnsAsync(Guid.NewGuid());

        var resultado = await _useCase.ExecutarAsync(request);

        Assert.NotNull(resultado);
        Assert.Equal(propostaId, resultado.PropostaId);
        Assert.NotEmpty(resultado.NumeroApolice);
        Assert.Equal(request.DataVigenciaInicio, resultado.DataVigenciaInicio);
        Assert.Equal(request.DataVigenciaFim, resultado.DataVigenciaFim);

        _contratacaoRepositoryMock.Verify(r => r.CriarAsync(It.IsAny<Contratacao>()), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_ComPropostaNaoEncontrada_DeveLancarExcecao()
    {
        var propostaId = Guid.NewGuid();
        var request = new CriarContratacaoRequest(
            propostaId,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        );

        _propostaServiceClientMock
            .Setup(c => c.ObterPropostaPorIdAsync(propostaId))
            .ReturnsAsync((PropostaInfo?)null);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));
        Assert.Contains("não encontrada", exception.Message);

        _contratacaoRepositoryMock.Verify(r => r.CriarAsync(It.IsAny<Contratacao>()), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_ComPropostaNaoAprovada_DeveLancarExcecao()
    {
        var propostaId = Guid.NewGuid();
        var request = new CriarContratacaoRequest(
            propostaId,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        );

        var propostaInfo = new PropostaInfo(
            propostaId,
            StatusProposta.EmAnalise,
            "João Silva",
            "Vida",
            100000m,
            5000m
        );

        _propostaServiceClientMock
            .Setup(c => c.ObterPropostaPorIdAsync(propostaId))
            .ReturnsAsync(propostaInfo);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));
        Assert.Contains("aprovadas podem ser contratadas", exception.Message);

        _contratacaoRepositoryMock.Verify(r => r.CriarAsync(It.IsAny<Contratacao>()), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_ComPropostaJaContratada_DeveLancarExcecao()
    {
        var propostaId = Guid.NewGuid();
        var request = new CriarContratacaoRequest(
            propostaId,
            DateTime.Today.AddDays(1),
            DateTime.Today.AddYears(1)
        );

        var propostaInfo = new PropostaInfo(
            propostaId,
            StatusProposta.Aprovada,
            "João Silva",
            "Vida",
            100000m,
            5000m
        );

        _propostaServiceClientMock
            .Setup(c => c.ObterPropostaPorIdAsync(propostaId))
            .ReturnsAsync(propostaInfo);

        _contratacaoRepositoryMock
            .Setup(r => r.ExistePorPropostaIdAsync(propostaId))
            .ReturnsAsync(true);

        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));
        Assert.Contains("já foi contratada", exception.Message);

        _contratacaoRepositoryMock.Verify(r => r.CriarAsync(It.IsAny<Contratacao>()), Times.Never);
    }
}