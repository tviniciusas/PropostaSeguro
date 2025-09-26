using System.Text.RegularExpressions;

namespace PropostaService.Domain.ValueObjects;

public record CPF
{
    private static readonly Regex CpfRegex = new(@"^\d{11}$", RegexOptions.Compiled);

    public string Valor { get; }

    public CPF(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("CPF não pode ser nulo ou vazio", nameof(valor));

        var cpfLimpo = valor.Replace(".", "").Replace("-", "").Trim();

        if (!CpfRegex.IsMatch(cpfLimpo))
            throw new ArgumentException("CPF deve conter exatamente 11 dígitos", nameof(valor));

        if (!ValidarCpf(cpfLimpo))
            throw new ArgumentException("CPF inválido", nameof(valor));

        Valor = cpfLimpo;
    }

    private static bool ValidarCpf(string cpf)
    {
        if (cpf.All(c => c == cpf[0]))
            return false;

        var multiplicadores1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        var multiplicadores2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

        var soma = cpf[..9].Select((c, i) => int.Parse(c.ToString()) * multiplicadores1[i]).Sum();
        var resto = soma % 11;
        var digito1 = resto < 2 ? 0 : 11 - resto;

        if (int.Parse(cpf[9].ToString()) != digito1)
            return false;

        soma = cpf[..10].Select((c, i) => int.Parse(c.ToString()) * multiplicadores2[i]).Sum();
        resto = soma % 11;
        var digito2 = resto < 2 ? 0 : 11 - resto;

        return int.Parse(cpf[10].ToString()) == digito2;
    }

    public override string ToString() => Valor;

    public static implicit operator string(CPF cpf) => cpf.Valor;
}