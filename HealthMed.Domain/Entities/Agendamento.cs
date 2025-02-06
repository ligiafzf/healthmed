namespace HealthMed.Api.Entities;

public class Agendamento
{
    public int Id { get; private set; }
    public int MedicoId { get; private set; }
    public int PacienteId { get; private set; }
    public DateTime DataHora { get; private set; }
    public bool Aprovado { get; private set; } = false;

    public Paciente Paciente { get; set; }
    public Medico Medico { get; set; }

    public Agendamento(int medicoId, int pacienteId, DateTime dataHora)
    {
        MedicoId = medicoId;
        PacienteId = pacienteId;
        DataHora = dataHora;
    }

    public void AceitarAgendamento()
    {
        Aprovado = true;
    }
}
