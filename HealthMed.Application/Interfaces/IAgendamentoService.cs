using HealthMed.Api.Dtos;

namespace HealthMed.Api.Interfaces;

public interface IAgendamentoService
{
    Task<IEnumerable<ListarMedicoDto>> ListarMedicos();
    Task<IEnumerable<ListarAgendamentoDto>> Listar(int medicoId, StatusAgendamento status);
    Task<IEnumerable<DateTime>> ObterHorariosDisponiveis(int medicoId, DateTime data);
    Task Criar(CadastroAgendamentoDto dto);
    Task Aceitar(int agendamentoId);
    Task Rejeitar(int agendamentoId);
}
