using System.Text.RegularExpressions;

namespace PropostaService.Domain.ValueObjects;

public record Email
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Valor { get; }

    public Email(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new ArgumentException("Email não pode ser nulo ou vazio", nameof(valor));

        var emailLimpo = valor.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(emailLimpo))
            throw new ArgumentException("Email inválido", nameof(valor));

        Valor = emailLimpo;
    }

    public override string ToString() => Valor;

    public static implicit operator string(Email email) => email.Valor;
}