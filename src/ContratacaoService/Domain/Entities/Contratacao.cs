namespace ContratacaoService.Domain.Entities;

public class Contratacao
{
    public Guid Id { get; private set; }
    public Guid PropostaId { get; private set; }
    public DateTime DataContratacao { get; private set; }
    public string NumeroApolice { get; private set; }
    public DateTime DataVigenciaInicio { get; private set; }
    public DateTime DataVigenciaFim { get; private set; }

    private Contratacao() { }

    public Contratacao(
        Guid propostaId,
        string numeroApolice,
        DateTime dataVigenciaInicio,
        DateTime dataVigenciaFim)
    {
        ValidarParametros(propostaId, numeroApolice, dataVigenciaInicio, dataVigenciaFim);

        Id = Guid.NewGuid();
        PropostaId = propostaId;
        DataContratacao = DateTime.UtcNow;
        NumeroApolice = numeroApolice.Trim();
        DataVigenciaInicio = dataVigenciaInicio;
        DataVigenciaFim = dataVigenciaFim;
    }

    private static void ValidarParametros(
        Guid propostaId,
        string numeroApolice,
        DateTime dataVigenciaInicio,
        DateTime dataVigenciaFim)
    {
        if (propostaId == Guid.Empty)
            throw new ArgumentException("ID da proposta é obrigatório", nameof(propostaId));

        if (string.IsNullOrWhiteSpace(numeroApolice))
            throw new ArgumentException("Número da apólice é obrigatório", nameof(numeroApolice));

        if (dataVigenciaInicio >= dataVigenciaFim)
            throw new ArgumentException("Data de início deve ser anterior à data de fim", nameof(dataVigenciaInicio));

        if (dataVigenciaInicio < DateTime.Today)
            throw new ArgumentException("Data de vigência não pode ser no passado", nameof(dataVigenciaInicio));
    }
}