namespace PropostaService.Domain.ValueObjects;

public record Dinheiro
{
    public decimal Valor { get; }

    public Dinheiro(decimal valor)
    {
        if (valor < 0)
            throw new ArgumentException("Valor não pode ser negativo", nameof(valor));

        Valor = Math.Round(valor, 2);
    }

    public static Dinheiro operator +(Dinheiro a, Dinheiro b) => new(a.Valor + b.Valor);
    public static Dinheiro operator -(Dinheiro a, Dinheiro b) => new(a.Valor - b.Valor);
    public static Dinheiro operator *(Dinheiro a, decimal multiplicador) => new(a.Valor * multiplicador);
    public static Dinheiro operator /(Dinheiro a, decimal divisor) => new(a.Valor / divisor);

    public static bool operator >(Dinheiro a, Dinheiro b) => a.Valor > b.Valor;
    public static bool operator <(Dinheiro a, Dinheiro b) => a.Valor < b.Valor;
    public static bool operator >=(Dinheiro a, Dinheiro b) => a.Valor >= b.Valor;
    public static bool operator <=(Dinheiro a, Dinheiro b) => a.Valor <= b.Valor;

    public override string ToString() => Valor.ToString("C2");

    public static implicit operator decimal(Dinheiro dinheiro) => dinheiro.Valor;
}