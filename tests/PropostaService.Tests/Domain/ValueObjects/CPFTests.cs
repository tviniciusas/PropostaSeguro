using Xunit;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Tests.Domain.ValueObjects;

public class CPFTests
{
    [Theory]
    [InlineData("12345678901")]
    [InlineData("11144477735")]
    public void CPF_ComValorValido_DeveCriarInstancia(string cpfValido)
    {
        var cpf = new CPF(cpfValido);

        Assert.Equal(cpfValido, cpf.Valor);
    }

    [Theory]
    [InlineData("123.456.789-01")]
    [InlineData("111.444.777-35")]
    public void CPF_ComFormatacao_DeveRemoverFormatacao(string cpfFormatado)
    {
        var cpfLimpo = cpfFormatado.Replace(".", "").Replace("-", "");
        var cpf = new CPF(cpfFormatado);

        Assert.Equal(cpfLimpo, cpf.Valor);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void CPF_ComValorNuloOuVazio_DeveLancarExcecao(string cpfInvalido)
    {
        Assert.Throws<ArgumentException>(() => new CPF(cpfInvalido));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("12345")]
    [InlineData("123456789012")]
    public void CPF_ComTamanhoIncorreto_DeveLancarExcecao(string cpfInvalido)
    {
        Assert.Throws<ArgumentException>(() => new CPF(cpfInvalido));
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("22222222222")]
    [InlineData("00000000000")]
    public void CPF_ComDigitosIguais_DeveLancarExcecao(string cpfInvalido)
    {
        Assert.Throws<ArgumentException>(() => new CPF(cpfInvalido));
    }

    [Theory]
    [InlineData("12345678900")]
    [InlineData("98765432100")]
    public void CPF_ComDigitoVerificadorIncorreto_DeveLancarExcecao(string cpfInvalido)
    {
        Assert.Throws<ArgumentException>(() => new CPF(cpfInvalido));
    }
}