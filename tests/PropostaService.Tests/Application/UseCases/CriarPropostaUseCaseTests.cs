using Xunit;
using Moq;
using PropostaService.Application.DTOs;
using PropostaService.Application.Ports;
using PropostaService.Application.UseCases;
using PropostaService.Domain.Entities;
using PropostaService.Domain.Enums;

namespace PropostaService.Tests.Application.UseCases;

public class CriarPropostaUseCaseTests
{
    private readonly Mock<IPropostaRepository> _repositoryMock;
    private readonly CriarPropostaUseCase _useCase;

    public CriarPropostaUseCaseTests()
    {
        _repositoryMock = new Mock<IPropostaRepository>();
        _useCase = new CriarPropostaUseCase(_repositoryMock.Object);
    }

    [Fact]
    public async Task ExecutarAsync_ComRequestValido_DeveCriarProposta()
    {
        var request = new CriarPropostaRequest(
            "João Silva",
            "12345678901",
            "joao@exemplo.com",
            TipoSeguro.Vida,
            100000m,
            5000m,
            "Proposta para seguro de vida"
        );

        _repositoryMock
            .Setup(r => r.CriarAsync(It.IsAny<Proposta>()))
            .ReturnsAsync(Guid.NewGuid());

        var resultado = await _useCase.ExecutarAsync(request);

        Assert.NotNull(resultado);
        Assert.Equal(request.NomeCliente, resultado.NomeCliente);
        Assert.Equal(request.CpfCliente, resultado.CpfCliente);
        Assert.Equal(request.EmailCliente, resultado.EmailCliente);
        Assert.Equal(request.TipoSeguro, resultado.TipoSeguro);
        Assert.Equal(request.ValorCobertura, resultado.ValorCobertura);
        Assert.Equal(request.ValorPremio, resultado.ValorPremio);
        Assert.Equal(StatusProposta.EmAnalise, resultado.Status);

        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Proposta>()), Times.Once);
    }

    [Fact]
    public async Task ExecutarAsync_ComCpfInvalido_DeveLancarExcecao()
    {
        var request = new CriarPropostaRequest(
            "João Silva",
            "cpf-inválido",
            "joao@exemplo.com",
            TipoSeguro.Vida,
            100000m,
            5000m
        );

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));

        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Proposta>()), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_ComEmailInvalido_DeveLancarExcecao()
    {
        var request = new CriarPropostaRequest(
            "João Silva",
            "12345678901",
            "email-inválido",
            TipoSeguro.Vida,
            100000m,
            5000m
        );

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));

        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Proposta>()), Times.Never);
    }

    [Fact]
    public async Task ExecutarAsync_ComValorPremioMaiorQueCobertura_DeveLancarExcecao()
    {
        var request = new CriarPropostaRequest(
            "João Silva",
            "12345678901",
            "joao@exemplo.com",
            TipoSeguro.Vida,
            5000m,
            10000m
        );

        await Assert.ThrowsAsync<ArgumentException>(() => _useCase.ExecutarAsync(request));

        _repositoryMock.Verify(r => r.CriarAsync(It.IsAny<Proposta>()), Times.Never);
    }
}