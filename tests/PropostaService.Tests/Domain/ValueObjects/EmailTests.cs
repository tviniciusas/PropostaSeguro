using Xunit;
using PropostaService.Domain.ValueObjects;

namespace PropostaService.Tests.Domain.ValueObjects;

public class EmailTests
{
    [Theory]
    [InlineData("teste@exemplo.com")]
    [InlineData("usuario.teste@dominio.com.br")]
    [InlineData("user+tag@example.org")]
    public void Email_ComValorValido_DeveCriarInstancia(string emailValido)
    {
        var email = new Email(emailValido);

        Assert.Equal(emailValido.ToLowerInvariant(), email.Valor);
    }

    [Theory]
    [InlineData("  TESTE@EXEMPLO.COM  ")]
    [InlineData("Usuario@Dominio.COM")]
    public void Email_ComEspacosEMaiusculas_DeveNormalizarValor(string emailComFormatacao)
    {
        var emailEsperado = emailComFormatacao.Trim().ToLowerInvariant();
        var email = new Email(emailComFormatacao);

        Assert.Equal(emailEsperado, email.Valor);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Email_ComValorNuloOuVazio_DeveLancarExcecao(string emailInvalido)
    {
        Assert.Throws<ArgumentException>(() => new Email(emailInvalido));
    }

    [Theory]
    [InlineData("emailinvalido")]
    [InlineData("@dominio.com")]
    [InlineData("email@")]
    [InlineData("email.dominio.com")]
    [InlineData("email@dominio")]
    public void Email_ComFormatoInvalido_DeveLancarExcecao(string emailInvalido)
    {
        Assert.Throws<ArgumentException>(() => new Email(emailInvalido));
    }
}