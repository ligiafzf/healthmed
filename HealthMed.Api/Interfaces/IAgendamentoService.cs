using HealthMed.Api.Dtos;

namespace HealthMed.Api.Interfaces;

public interface IAgendamentoService
{
    Task<IEnumerable<ListarMedicoDto>> ListarMedicos();
    Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data);
    Task CriarAgendamento(int medicoId, int pacienteId, DateTime dataHora);
    Task AceitarAgendamento(int agendamentoId);
}
