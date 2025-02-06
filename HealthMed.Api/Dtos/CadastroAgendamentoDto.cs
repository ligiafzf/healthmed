namespace HealthMed.Api.Dtos;

public class CadastroAgendamentoDto
{
    public int MedicoId { get; set; }
    public int PacienteId { get; set; }
    public DateTime DataHora { get; set; }
}
